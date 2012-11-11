﻿using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Windows;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Threading;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Settings;
using System.Collections.Generic;

namespace WhatsShakingNZ
{
    public partial class MainPage : WhatsShakingBasePage
    {
        private enum ButtonNames { RefreshButton = 0, MapButton };
        
        public MainPage()
        {
            InitializeComponent();
            InitializeApplicationBar();
            this.DataContext = QuakeContainer;
            SetBackgroundImage();
            LittleWatson.CheckForPreviousException();
            if (QuakeContainer.AppSettings.ShowLiveTileSetting)
                ScheduledTaskHelper.Update();
            // TODO Handle no data connectivity! Primarily serialization & storage of most recent seen quakes list.
        }

        private void SetBackgroundImage()
        {
            ImageBrush backgroundImage = new ImageBrush();
            backgroundImage.ImageSource = ShakingHelper.GetBackgroundImage();
            LayoutRoot.Background = backgroundImage;
        }

        private void InitializeApplicationBar()
        {
            List<ApplicationBarIconButton> buttons = new List<ApplicationBarIconButton>();
            
            ApplicationBarIconButton refreshButton = new ApplicationBarIconButton();
            refreshButton.Text = "refresh";
            refreshButton.IconUri = new Uri("/Icons/appbar.refresh.rest.png", UriKind.Relative);
            refreshButton.Click += RefreshRecentButton_Click;
            
            ApplicationBarIconButton mapViewButton = new ApplicationBarIconButton();
            mapViewButton.Text = "map view";
            mapViewButton.IconUri = new Uri("/Icons/appbar.map.png", UriKind.Relative);
            mapViewButton.Click += MapPageButton_Click;
            
            buttons.Add(refreshButton);
            buttons.Add(mapViewButton);
            
            base.InitializeApplicationBar(buttons);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
                GetQuakes();
            QuakeContainer.RefreshViewsIfSettingsChanged();
        }

        protected override void GetQuakes()
        {
            (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = false;
            customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Visible;
            customIndeterminateProgressBar.IsIndeterminate = true;
            QuakeContainer.DownloadNewQuakes();
        }

        public override void QuakesUpdatedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == "Quakes")
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = true;
                    customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                    customIndeterminateProgressBar.IsIndeterminate = false;
                    if (e == null)
                    {
                        ToastPrompt toast = new ToastPrompt()
                        {
                            TextOrientation = System.Windows.Controls.Orientation.Vertical,
                            Title = "problem retrieving quakes",
                            Message = "please check your data connection is working"
                        };
                        toast.Show();
                    }
                });
            }
        }

        private void MapPageButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.Relative));
        }

        protected override void QuakeItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ContentPanel.SelectedIndex >= 0)
            {
                int selectedIndex = ContentPanel.SelectedIndex;
                ContentPanel.SelectedItem = null;
                NavigateToQuakePage(selectedIndex);
            }
        }

    }
}