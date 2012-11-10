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

namespace WhatsShakingNZ
{
    public partial class QuakeDisplayPage : PhoneApplicationPage
    {
        //private AppSettings appSettings;
        public QuakeDisplayPage()
        {
            //appSettings = new AppSettings();
            //InitializeComponent();
            //ContentPanel.DataContext = App.SelectedQuake;
            //GeoCoordinate location = new GeoCoordinate(App.SelectedQuake.Location.Latitude, App.SelectedQuake.Location.Longitude);
            //QuakeMap.Center = location;
            //Pushpin pin = new Pushpin()
            //        {
            //            Location = new GeoCoordinate(App.SelectedQuake.Location.Latitude, App.SelectedQuake.Location.Longitude),
            //            Content = App.SelectedQuake.FormattedMagnitude
            //        };
            //if (App.SelectedQuake.Magnitude >= appSettings.MinimumWarningMagnitudeSetting)
            //    pin.Background = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            //QuakeMap.Children.Add(pin);
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