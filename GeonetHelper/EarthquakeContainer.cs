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
using System.Collections.ObjectModel;
using WhatsShakingNZ.Settings;
using System.ComponentModel;

namespace WhatsShakingNZ.GeonetHelper
{
    public sealed class EarthquakeContainer : INotifyPropertyChanged
    {
        public EarthquakeContainer()
        {
            GeonetAccessor.GetQuakesCompletedEvent += QuakeListener;
        }

        public void QuakeListener(object sender, QuakeEventArgs e)
        {
            Quakes = e.Quakes;   
        }

        private static AppSettings _appSettings;
        public AppSettings AppSettings
        {
            get
            {
                if (null == _appSettings)
                {
                    _appSettings = new AppSettings();
                    AppSettings.SettingsChangedEvent += SettingsChanged;
                }
                return _appSettings;
            }
        }

        private ObservableCollection<Earthquake> _quakes;
        public ObservableCollection<Earthquake> Quakes
        {
            get
            {
                if (null == _quakes)
                    _quakes = new ObservableCollection<Earthquake>();
                return QuakeFilter.GetFilteredQuakes(_quakes, AppSettings);
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

        public static Earthquake SelectedQuake { get; set; }

        private bool _settingsChanged;
        public void SettingsChanged()
        {
            _settingsChanged = true;
        }

        public void RefreshViewsIfSettingsChanged()
        {
            if (_settingsChanged)
                RefreshViews();
            _settingsChanged = false;
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
