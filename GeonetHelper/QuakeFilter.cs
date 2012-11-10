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
using WhatsShakingNZ.Settings;

namespace WhatsShakingNZ.GeonetHelper
{
    public static class QuakeFilter
    {
        /// <summary>
        /// Filters a provided collection of quakes. The returned ObservableCollection contains
        /// only quakes that are lower in magnitude than the MinimumDisplayMagnitudeSetting,
        /// and will contain less than or equal to NumberOfQuakesToShowSetting.
        /// </summary>
        /// <param name="quakes"></param>
        /// <returns></returns>
        public static ObservableCollection<Earthquake> GetFilteredQuakes(IEnumerable<Earthquake> quakes, AppSettings appSettings)
        {
            return new ObservableCollection<Earthquake>(quakes.Where(q => q.Magnitude >= appSettings.MinimumDisplayMagnitudeSetting).Take(appSettings.NumberOfQuakesToShowSetting));
        }

    }
}
