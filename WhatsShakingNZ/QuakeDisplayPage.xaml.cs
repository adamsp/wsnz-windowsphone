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
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using WhatsShakingNZ.GeonetHelper;

namespace WhatsShakingNZ
{
    public partial class QuakeDisplayPage : PhoneApplicationPage
    {
        public QuakeDisplayPage()
        {
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
            if (quake.Magnitude >= (Application.Current as App).EarthquakeContainer.AppSettings.MinimumWarningMagnitudeSetting)
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