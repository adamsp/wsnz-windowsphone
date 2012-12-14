using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
