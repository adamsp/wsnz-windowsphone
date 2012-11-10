using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.ComponentModel;
using System.Device.Location;
using Coding4Fun.Phone.Controls;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Settings;

namespace WhatsShakingNZ
{
    public partial class MapPage : PhoneApplicationPage
    {
        private enum ButtonNames { ZoomOutButton = 0, RefreshButton, ListButton, ZoomInButton };
        private EarthquakeContainer quakeContainer;
        public MapPage()
        {
            quakeContainer = (Application.Current as App).EarthquakeContainer;
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            quakeContainer.PropertyChanged += MapUpdater;
            quakeContainer.RefreshViews();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            quakeContainer.PropertyChanged -= MapUpdater;
            base.OnNavigatedFrom(e);
        }

        public void MapUpdater(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == "Quakes")
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = true;
                    customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                    customIndeterminateProgressBar.IsIndeterminate = false;
                    if (e != null)
                    {
                        UpdateMap();
                    }
                    else
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

        private void UpdateMap()
        {
            QuakeMap.Children.Clear();
            // Select only quakes above the settings magnitude
            // The OrderBy means that higher magnitude quakes will be pinned on top OrderBy(s => s.Magnitude)
            double minWarningMagnitude = quakeContainer.AppSettings.MinimumWarningMagnitudeSetting;
            if (quakeContainer.Quakes != null)
            {
                foreach (var q in quakeContainer.Quakes.Reverse())
                {
                    Pushpin pin = new Pushpin()
                    {
                        Location = new GeoCoordinate(q.Location.Latitude, q.Location.Longitude),
                        Content = q.FormattedMagnitude,
                        DataContext = q,
                    };
                    pin.Tap += QuakePin_Tap;
                    if (q.Magnitude >= minWarningMagnitude)
                        pin.Background = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                    QuakeMap.Children.Add(pin);
                }
            }
        }

        private void GetQuakes()
        {
           (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = false;
            customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Visible;
            customIndeterminateProgressBar.IsIndeterminate = true;
            quakeContainer.DownloadNewQuakes();
        }

        private void QuakePin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            //App.SelectedQuake = pin.DataContext as Earthquake;
            NavigationService.Navigate(new Uri("/QuakeDisplayPage.xaml", UriKind.Relative));
        }

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            double zoom;
            zoom = QuakeMap.ZoomLevel;
            QuakeMap.ZoomLevel = ++zoom;
        }

        private void RefreshRecentButton_Click(object sender, EventArgs e)
        {
            GetQuakes();
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

        private void SettingsPageMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void AboutPageMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
    }
}