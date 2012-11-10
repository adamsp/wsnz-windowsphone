using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using WhatsShakingNZ.GeonetHelper;
using System.Collections.Generic;

namespace WhatsShakingNZ
{
    public static class ShakingHelper
    {
        public static readonly SolidColorBrush WarningColour = new SolidColorBrush(Colors.Red);
        public static readonly string PeriodicTaskName = "WhatsShakingNZPeriodicTask";
        private static readonly BitmapImage darkBackground = new BitmapImage(new Uri("Images/background_dark.png", UriKind.Relative));
        private static readonly BitmapImage lightBackground = new BitmapImage(new Uri("Images/background_light.png", UriKind.Relative));
        static ShakingHelper() { }

        public static BitmapImage GetBackgroundImage()
        {
            if ((Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"] == System.Windows.Visibility.Visible)
                return darkBackground;
            else
                return lightBackground;
        }
    }
}
