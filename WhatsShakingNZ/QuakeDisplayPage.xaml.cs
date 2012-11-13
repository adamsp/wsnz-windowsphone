using System;
using System.Device.Location;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Settings;
using Coding4Fun.Phone.Controls;

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
            GeoCoordinate location = new GeoCoordinate(quake.Location.Latitude, quake.Location.Longitude);
            QuakeMap.Center = location;
            Pushpin pin = new Pushpin()
            {
                Location = new GeoCoordinate(quake.Location.Latitude, quake.Location.Longitude),
                Content = quake.FormattedMagnitude
            };
            if (quake.Magnitude >= appSettings.MinimumWarningMagnitudeSetting)
                pin.Background = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            QuakeMap.Children.Add(pin);
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
                toastMessage = "This earthquake has been reviewed and confirmed by a duty officer or analyst.";
            }
            else if (quake.Status.Equals("automatic", StringComparison.OrdinalIgnoreCase))
            {
                toastMessage = "An automatic earthquake location that has not been reviewed by a duty officer or analyst.";
            }
            else if (quake.Status.Equals("deleted", StringComparison.OrdinalIgnoreCase))
            {
                toastMessage = "Oops! This was not a real earthquake and has been deleted.";
            }
            else if (quake.Status.Equals("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                toastMessage = "This earthquake location is a duplicate of another one.";
            }
            else
            {
                toastMessage = "We don't know what this status means! :(";
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
    }
}