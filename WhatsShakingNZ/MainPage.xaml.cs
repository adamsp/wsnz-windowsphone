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
using WhatsShakingNZ.Settings;

namespace WhatsShakingNZ
{
    public partial class MainPage : PhoneApplicationPage
    {
        private enum ButtonNames { RefreshButton = 0, MapButton };
        private EarthquakeContainer quakeContainer;
        
        public MainPage()
        {
            quakeContainer = (Application.Current as App).EarthquakeContainer;
            InitializeComponent();
            this.DataContext = quakeContainer;
            ImageBrush backgroundImage = new ImageBrush();
            backgroundImage.ImageSource = ShakingHelper.GetBackgroundImage();
            LayoutRoot.Background = backgroundImage;
            LittleWatson.CheckForPreviousException();
            if (quakeContainer.AppSettings.ShowLiveTileSetting)
                ScheduledTaskHelper.Update();
            // TODO Handle no data connectivity! Primarily serialization & storage of most recent seen quakes list.
            // TODO Add Geonet Beta functionality
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            quakeContainer.PropertyChanged += ListUpdater;
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
                GetQuakes();
            quakeContainer.RefreshViewsIfSettingsChanged();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            quakeContainer.PropertyChanged -= ListUpdater;
            base.OnNavigatedFrom(e);
        }
        
        private void GetQuakes()
        {
            (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = false;
            customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Visible;
            customIndeterminateProgressBar.IsIndeterminate = true;
            quakeContainer.DownloadNewQuakes();
        }

        public void ListUpdater(object sender, PropertyChangedEventArgs e)
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
                int selectedIndex = ContentPanel.SelectedIndex;
                string navUri = string.Format("/QuakeDisplayPage.xaml?selectedItem={0}", selectedIndex);
                ContentPanel.SelectedItem = null;
                NavigationService.Navigate(new Uri(navUri, UriKind.Relative));
            }
        }

    }
}