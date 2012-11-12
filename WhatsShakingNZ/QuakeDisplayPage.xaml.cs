using System;
using System.Device.Location;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Settings;

namespace WhatsShakingNZ
{
    public partial class QuakeDisplayPage : PhoneApplicationPage
    {
        private AppSettings appSettings;
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
            
            Earthquake quake = (Application.Current as App).EarthquakeContainer.Quakes[index];

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
    }
}