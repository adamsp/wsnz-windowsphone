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

        public EarthquakeContainer() : this(new HttpWebRequestFactory())
        {
        }

        public EarthquakeContainer(IHttpWebRequestFactory factory)
        {
            appSettings = new AppSettings();
            geonet = new GeonetAccessor(factory);
            geonet.GetQuakesCompletedEvent += QuakeListener;
        }

        private void QuakeListener(object sender, QuakeEventArgs e)
        {
            // TODO Show different messages depending on e.Status
            if (e != null)
            {
                Status = e.Status;
                switch (e.Status)
                {
                    case GeonetSuccessStatus.Success:
                        Quakes = e.Quakes;
                        break;
                    case GeonetSuccessStatus.BadGeonetData:
                    case GeonetSuccessStatus.NetworkFailure:
                    case GeonetSuccessStatus.NoGeonetData:
                        Quakes = null;
                        break;
                }
            }
        }

        public GeonetSuccessStatus Status
        {
            get;
            set;
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
                _quakes = value;
                NotifyPropertyChanged(QuakesUpdatedEventKey);
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
