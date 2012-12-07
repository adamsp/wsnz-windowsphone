using System;
using System.Device.Location;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Settings;
using Coding4Fun.Phone.Controls;
using WhatsShakingNZ.Localization;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;

namespace WhatsShakingNZ
{
    public partial class QuakeDisplayPage : PhoneApplicationPage
    {
        private AppSettings appSettings;
        private Earthquake quake;
        public QuakeDisplayPage()
        {
            appSettings = new AppSettings();
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string selectedIndex = string.Empty;
            int index;
            if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
            {
                index = int.Parse(selectedIndex);
            }
            else return;
            
            quake = (Application.Current as App).EarthquakeContainer.Quakes[index];

            ContentPanel.DataContext = quake;

            QuakeMap.Center = quake.Location;
            Pushpin pin = new Pushpin
            {
                GeoCoordinate = quake.Location,
                Content = quake.FormattedMagnitude
            };
            if (quake.Magnitude >= appSettings.MinimumWarningMagnitudeSetting)
                pin.Background = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;

            MapOverlay overlay = new MapOverlay();
            overlay.Content = pin;
            overlay.GeoCoordinate = quake.Location;
            overlay.PositionOrigin = new Point(0, 1);

            MapLayer layer = new MapLayer();
            layer.Add(overlay);
            QuakeMap.Layers.Add(layer);

            base.OnNavigatedTo(e);
        }

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            double zoom;
            zoom = QuakeMap.ZoomLevel;
            QuakeMap.ZoomLevel = ++zoom;
        }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            double zoom;
            zoom = QuakeMap.ZoomLevel;
            QuakeMap.ZoomLevel = --zoom;
        }

        private void StatusTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            /**
             * These messages come from Geonet. If you go to http://www.geonet.org.nz/quakes/region/newzealand
             * and go to any quake page, then click the little question mark next to "Status", these
             * messages popup in a wee window.
             */
            if (null == quake) return;
            string toastMessage;
            if (quake.Status.Equals("reviewed", StringComparison.OrdinalIgnoreCase))
            {
                toastMessage = AppResources.EarthquakeStatusReviewed;
            }
            else if (quake.Status.Equals("automatic", StringComparison.OrdinalIgnoreCase))
            {
                toastMessage = AppResources.EarthquakeStatusAutomatic;
            }
            else if (quake.Status.Equals("deleted", StringComparison.OrdinalIgnoreCase))
            {
                toastMessage = AppResources.EarthquakeStatusDeleted;
            }
            else if (quake.Status.Equals("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                toastMessage = AppResources.EarthquakeStatusDuplicate;
            }
            else
            {
                toastMessage = AppResources.EarthquakeStatusUnknown;
            }
            ToastPrompt toast = new ToastPrompt()
            {
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Title = quake.Status,
                Message = toastMessage
            };
            toast.Show();
        }

        private void QuakeMap_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.ApplicationId = ShakingHelper.MapsApplicationId;
            Microsoft.Phone.Maps.MapsSettings.ApplicationContext.AuthenticationToken = ShakingHelper.MapsAuthenticationToken;
        }
    }
}