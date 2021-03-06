﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Localization;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;

namespace WhatsShakingNZ
{
    public partial class MapPage : WhatsShakingBasePage
    {
        private enum ButtonNames { ZoomOutButton = 0, RefreshButton, ZoomInButton };
        
        public MapPage() : base()
        {
            InitializeComponent();
            InitializeApplicationBar();
        }

        private void InitializeApplicationBar()
        {
            /***
             * Make sure these buttons are added in the same order as the button names in the 
             * enumeration above. Otherwise we don't know which button is where - see GetQuakes().
             **/
            List<ApplicationBarIconButton> buttons = new List<ApplicationBarIconButton>();

            ApplicationBarIconButton zoomOutButton = new ApplicationBarIconButton();
            zoomOutButton.Text = AppResources.AppBarZoomOutButtonText;
            zoomOutButton.IconUri = new Uri("/Icons/appbar.minus.rest.png", UriKind.Relative);
            zoomOutButton.Click += ZoomOutButton_Click;

            ApplicationBarIconButton refreshButton = new ApplicationBarIconButton();
            refreshButton.Text = AppResources.AppBarRefreshButtonText;
            refreshButton.IconUri = new Uri("/Icons/appbar.refresh.rest.png", UriKind.Relative);
            refreshButton.Click += RefreshRecentButton_Click;

            //ApplicationBarIconButton listViewButton = new ApplicationBarIconButton();
            //listViewButton.Text = AppResources.AppBarListViewButtonText;
            //listViewButton.IconUri = new Uri("/Icons/appbar.list.png", UriKind.Relative);
            //listViewButton.Click += ListPageButton_Click;

            ApplicationBarIconButton zoomInButton = new ApplicationBarIconButton();
            zoomInButton.Text = AppResources.AppBarZoomInButtonText;
            zoomInButton.IconUri = new Uri("/Icons/appbar.add.rest.png", UriKind.Relative);
            zoomInButton.Click += ZoomInButton_Click;

            buttons.Add(zoomOutButton);
            buttons.Add(refreshButton);
            //buttons.Add(listViewButton);
            buttons.Add(zoomInButton);

            base.InitializeApplicationBar(buttons);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            RefreshViews();
        }

        private void UpdateMap()
        {
            QuakeMap.Layers.Clear();
            // Select only quakes above the settings magnitude
            // The OrderBy means that higher magnitude quakes will be pinned on top OrderBy(s => s.Magnitude)
            double minWarningMagnitude = AppSettingsForPage.MinimumWarningMagnitudeSetting;
            if (QuakeContainer.Quakes != null)
            {
                MapLayer layer = new MapLayer();
                // Use "reverse" here so newer quakes are on top.
                foreach (var q in QuakeContainer.Quakes.Reverse())
                {
                    Pushpin pin = new Pushpin()
                    {
                        GeoCoordinate = q.Location,
                        Content = q.FormattedMagnitude,
                        DataContext = q,
                    };
                    pin.Tap += QuakeItem_Tap;
                    if (q.Magnitude >= minWarningMagnitude)
                        pin.Background = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                    MapOverlay overlay = new MapOverlay();
                    overlay.Content = pin;
                    overlay.GeoCoordinate = q.Location;
                    overlay.PositionOrigin = new Point(0, 1);

                    layer.Add(overlay);
                    
                }
                QuakeMap.Layers.Add(layer);
            }
        }

        protected override void StartGetQuakes()
        {
            /**
             * Can't put this in the base class because the refresh button has no identifier - so we can't
             * just find it in the collection. Ugh.
             * */
           (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = false;
            customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Visible;
            customIndeterminateProgressBar.IsIndeterminate = true;
        }

        protected override void GetQuakesFinished()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = true;
                customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                customIndeterminateProgressBar.IsIndeterminate = false;
                UpdateMap();
            });
        }

        protected override void QuakeItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            // Remember the children have been added in the reverse order to the order they appear in the original list.
            NavigateToQuakePage(pin.DataContext as Earthquake);
        }

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            double zoom;
            zoom = QuakeMap.ZoomLevel;
            QuakeMap.ZoomLevel = ++zoom;
        }

        private void ListPageButton_Click(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            double zoom;
            zoom = QuakeMap.ZoomLevel;
            QuakeMap.ZoomLevel = --zoom;
        }

        private void QuakeMap_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = ShakingHelper.MapsApplicationId;
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = ShakingHelper.MapsAuthenticationToken;
        }
    }
}