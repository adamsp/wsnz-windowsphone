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

namespace Settings
{
    public class DefaultSettings
    {
        // The default value of our settings
        public const double MinimumDisplayMagnitudeDefaultValue = 2.0;
        public const double MinimumWarningMagnitudeDefaultValue = 4.0;
        public const bool TwentyFourHourClockDefaultValue = false;
        public const bool ShowLiveTileDefaultValue = false;
        public const int NumberOfQuakesToShowDefaultValue = 10;
        public const bool UseGeonetAllQuakesEndpointDefaultValue = false;
    }
}
