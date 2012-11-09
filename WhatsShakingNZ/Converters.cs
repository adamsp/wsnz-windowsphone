using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WhatsShakingNZ.GeonetHelper;

namespace WhatsShakingNZ
{
    /// <summary>
    /// Returns a Brush which is Red if the quake.Magnitude is over Settings.MinimumWarningValue
    /// </summary>
    public class FontColourConverter : System.Windows.Data.IValueConverter
    {
        private static AppSettings appSettings;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double magnitude = (double)value;
            if (appSettings == null)
                appSettings = new AppSettings();
            if (magnitude >= appSettings.MinimumWarningMagnitudeSetting)
                return App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            return App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }//End converter 

    /// <summary>
    /// Converts the Depth of the quake to be in 'Depth: X.XX kilometers' format
    /// </summary>
    public class DepthConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Earthquake quake = value as Earthquake;
            if (quake == null)
                return null;
            return String.Format("Depth: {0} kilometers", quake.FormattedDepth); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }//End converter 

    /// <summary>
    /// Converts the Depth of the quake to be in 'X km' format
    /// </summary>
    public class ShortDepthConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Earthquake quake = value as Earthquake;
            if (quake == null)
                return null;
            return String.Format("{0} km", quake.FormattedDepth); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }//End converter 

    /// <summary>
    /// Converts the Date of the quake to 24 or 12 hour format, depending on the value of settings.TwentyFourHourClockSetting.
    /// </summary>
    public class DateConverter : System.Windows.Data.IValueConverter
    {
        private static AppSettings settings;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Earthquake quake = value as Earthquake;
            if (quake == null)
                return null;
            if (settings == null)
                settings = new AppSettings();
            DateTime localTime = quake.Date.ToLocalTime();
            if (settings.TwentyFourHourClockSetting)
                return localTime.ToString("d") + " " + localTime.ToString("HH:mm:ss");
            else
                return localTime.ToString("g");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }//End converter

}
