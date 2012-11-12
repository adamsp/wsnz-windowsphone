using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;

namespace WhatsShakingNZ
{
    public partial class MapPage : WhatsShakingBasePage
    {
        private enum ButtonNames { ZoomOutButton = 0, RefreshButton, ListButton, ZoomInButton };
        
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
            zoomOutButton.Text = "zoom out";
            zoomOutButton.IconUri = new Uri("/Icons/appbar.minus.rest.png", UriKind.Relative);
            zoomOutButton.Click += ZoomOutButton_Click;

            ApplicationBarIconButton refreshButton = new ApplicationBarIconButton();
            refreshButton.Text = "refresh";
            refreshButton.IconUri = new Uri("/Icons/appbar.refresh.rest.png", UriKind.Relative);
            refreshButton.Click += RefreshRecentButton_Click;

            ApplicationBarIconButton listViewButton = new ApplicationBarIconButton();
            listViewButton.Text = "list view";
            listViewButton.IconUri = new Uri("/Icons/appbar.list.png", UriKind.Relative);
            listViewButton.Click += ListPageButton_Click;

            ApplicationBarIconButton zoomInButton = new ApplicationBarIconButton();
            zoomInButton.Text = "zoom in";
            zoomInButton.IconUri = new Uri("/Icons/appbar.add.rest.png", UriKind.Relative);
            zoomInButton.Click += ZoomInButton_Click;

            buttons.Add(zoomOutButton);
            buttons.Add(refreshButton);
            buttons.Add(listViewButton);
            buttons.Add(zoomInButton);

            base.InitializeApplicationBar(buttons);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            QuakeContainer.RefreshViews();
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
                    if (QuakeContainer.Quakes.Count > 0)
                    {
                        UpdateMap();
                    }
                    else
                    {
                        ShowNoConnectivityToast();
                    }
                });
            }
        }
        
        private void UpdateMap()
        {
            QuakeMap.Children.Clear();
            // Select only quakes above the settings magnitude
            // The OrderBy means that higher magnitude quakes will be pinned on top OrderBy(s => s.Magnitude)
            double minWarningMagnitude = AppSettingsForPage.MinimumWarningMagnitudeSetting;
            if (QuakeContainer.Quakes != null)
            {
                // Use "reverse" here so newer quakes are on top.
                foreach (var q in QuakeContainer.Quakes.Reverse())
                {
                    Pushpin pin = new Pushpin()
                    {
                        Location = new GeoCoordinate(q.Location.Latitude, q.Location.Longitude),
                        Content = q.FormattedMagnitude,
                        DataContext = q,
                    };
                    pin.Tap += QuakeItem_Tap;
                    if (q.Magnitude >= minWarningMagnitude)
                        pin.Background = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                    QuakeMap.Children.Add(pin);
                }
            }
        }

        protected override void GetQuakes()
        {
            /**
             * Can't put this in the base class because the refresh button has no identifier - so we can't
             * just find it in the collection. Ugh.
             * */
           (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = false;
            customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Visible;
            customIndeterminateProgressBar.IsIndeterminate = true;
            QuakeContainer.DownloadNewQuakes();
        }

        protected override void QuakeItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            // Remember the children have been added in the reverse order to the order they appear in the original list.
            int selectedIndex = (QuakeMap.Children.Count - 1) - QuakeMap.Children.IndexOf(pin);
            NavigateToQuakePage(selectedIndex);
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
    }
}