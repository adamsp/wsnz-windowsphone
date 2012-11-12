using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WhatsShakingNZ.Settings;

namespace WhatsShakingNZ.GeonetHelper
{
    public sealed class EarthquakeContainer : INotifyPropertyChanged
    {
        private AppSettings appSettings;

        public EarthquakeContainer()
        {
            appSettings = new AppSettings();
            GeonetAccessor.GetQuakesCompletedEvent += QuakeListener;
        }

        public void QuakeListener(object sender, QuakeEventArgs e)
        {
            Quakes = e.Quakes;   
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
                    NotifyPropertyChanged("Quakes");
                }
            }
        }

        public void RefreshViews()
        {
            NotifyPropertyChanged("Quakes");
        }

        public void DownloadNewQuakes()
        {
            GeonetAccessor.GetQuakes();
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
