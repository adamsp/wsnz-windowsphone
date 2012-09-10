using System;
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

namespace WhatsShakingNZ
{
    public partial class MainPage : PhoneApplicationPage
    {
        private enum ButtonNames { RefreshButton = 0, MapButton };
        private AppSettings appSettings;
        // Constructor
        public MainPage()
        {
            appSettings = new AppSettings();
            InitializeComponent();
            this.DataContext = Application.Current as App;
            ImageBrush backgroundImage = new ImageBrush();
            backgroundImage.ImageSource = ShakingHelper.GetBackgroundImage();
            LayoutRoot.Background = backgroundImage;
            LittleWatson.CheckForPreviousException();
            if (appSettings.ShowLiveTileSetting)
                ScheduledTaskHelper.Update();
            // TODO Handle no data connectivity! Primarily serialization & storage of most recent seen quakes list.
            // TODO Add Geonet Beta functionality
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            GeonetAccessor.GetQuakesCompletedEvent += QuakeListener;
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
                GetQuakes();
            else if ((Application.Current as App).SettingsChanged)
            {
                (Application.Current as App).SettingsChanged = false;
                // null check added after marketplace submission. Will only crash if someone edits settings before Geonet returns. Eek.
                if((Application.Current as App).AllLatestQuakes != null)
                    (Application.Current as App).Quakes = ShakingHelper.GetFilteredQuakes((Application.Current as App).AllLatestQuakes, appSettings);
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            GeonetAccessor.GetQuakesCompletedEvent -= QuakeListener;
            base.OnNavigatedFrom(e);
        }

        private void GetQuakes()
        {
            GeonetAccessor.GetQuakes();
            (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = false;
            customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Visible;
            customIndeterminateProgressBar.IsIndeterminate = true;
        }

        public void QuakeListener(object sender, QuakeEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = true;
                customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                customIndeterminateProgressBar.IsIndeterminate = false;
                if (e != null)
                {
                    (Application.Current as App).Quakes = ShakingHelper.GetFilteredQuakes(e.Quakes, appSettings);
                    (Application.Current as App).AllLatestQuakes = e.Quakes;
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

        private void RefreshRecentButton_Click(object sender, EventArgs e)
        {
            GetQuakes();
        }

        private void MapPageButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.Relative));
        }

        private void SettingsPageMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void AboutPageMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void ContentPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ContentPanel.SelectedIndex >= 0)
            {
                App.SelectedQuake = ContentPanel.SelectedItem as Earthquake;
                ContentPanel.SelectedItem = null;
                NavigationService.Navigate(new Uri("/QuakeDisplayPage.xaml", UriKind.Relative));
            }
        }

    }
}