using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WhatsShakingNZ
{
    public static class ShakingHelper
    {
        public static readonly SolidColorBrush WarningColour = new SolidColorBrush(Colors.Red);
        public static readonly string PeriodicTaskName = "WhatsShakingNZPeriodicTask";
        public static readonly string MapsApplicationId = "your_app_id";
        public static readonly string MapsAuthenticationToken = "your_auth_token";
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
