using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WhatsShakingNZ.Settings;
using HttpWebAdapters;

namespace WhatsShakingNZ.GeonetHelper
{
    public sealed class EarthquakeContainer : INotifyPropertyChanged
    {
        private AppSettings appSettings;
        private GeonetAccessor geonet;
        public const string QuakesUpdatedEventKey = "Quakes";

        public EarthquakeContainer()
        {
            appSettings = new AppSettings();
            geonet = new GeonetAccessor(new HttpWebRequestFactory());
            geonet.GetQuakesCompletedEvent += QuakeListener;
        }

        public void QuakeListener(object sender, QuakeEventArgs e)
        {
            if (e != null)
                Quakes = e.Quakes;
            else
                Quakes = null;
        }

        private ObservableCollection<Earthquake> _quakes;
        public ObservableCollection<Earthquake> Quakes
        {
            get
            {
                if (null == _quakes)
                    _quakes = new ObservableCollection<Earthquake>();
                return QuakeFilter.GetFilteredQuakes(_quakes, appSettings);
            }
            set
            {
                if (_quakes != value)
                {
                    _quakes = value;
                    NotifyPropertyChanged(QuakesUpdatedEventKey);
                }
            }
        }

        public void RefreshViews()
        {
            NotifyPropertyChanged(QuakesUpdatedEventKey);
        }

        public void DownloadNewQuakes()
        {
            geonet.GetQuakes();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                });
            }
        }
        #endregion
    }
}
