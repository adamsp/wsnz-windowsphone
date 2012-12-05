using System;
using System.IO.IsolatedStorage;
using Settings;

namespace WhatsShakingNZ.Settings
{
    public class AppSettings
    {
        // Our isolated storage settings
        IsolatedStorageSettings settings;

        public static SettingsChangedEventHandler SettingsChangedEvent;
        public delegate void SettingsChangedEventHandler();

        // The isolated storage key names of our settings
        const string MinimumDisplayMagnitudeKey = "MinimumDisplayMagnitude";
        const string MinimumWarningMagnitudeKey = "MinimumWarningMagnitude";
        const string TwentyFourHourClockKey = "TwentyFourHourClock";
        const string ShowLiveTileKey = "ShowLiveTile";
        const string NumberOfQuakesToShowKey = "QuakesToShow";
        const string UseGeonetAllQuakesEndpointKey = "UseGeonetAllQuakesEndpoint";

        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
        public AppSettings()
        {
            // Get the settings for this application.
            try
            {
                settings = IsolatedStorageSettings.ApplicationSettings;
            }
            catch (System.IO.IsolatedStorage.IsolatedStorageException e)
            {
                // handle exception
                // Will use defaults if we can't get the app settings.
            }
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings != null && settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            settings.Save();
            if (null != SettingsChangedEvent)
                SettingsChangedEvent();
        }


        /// <summary>
        /// Property to get and set the Minimum Display Magnitude.
        /// </summary>
        public double MinimumDisplayMagnitudeSetting
        {
            get
            {
                return GetValueOrDefault<double>(MinimumDisplayMagnitudeKey, DefaultSettings.MinimumDisplayMagnitudeDefaultValue);
            }
            set
            {
                if (AddOrUpdateValue(MinimumDisplayMagnitudeKey, value))
                {
                    Save();
                }
            }
        }


        /// <summary>
        /// Property to get and set the Minimum Warning Magnitude.
        /// </summary>
        public double MinimumWarningMagnitudeSetting
        {
            get
            {
                return GetValueOrDefault<double>(MinimumWarningMagnitudeKey, DefaultSettings.MinimumWarningMagnitudeDefaultValue);
            }
            set
            {
                if (AddOrUpdateValue(MinimumWarningMagnitudeKey, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set the number of quakes to show setting.
        /// </summary>
        public int NumberOfQuakesToShowSetting
        {
            get
            {
                return GetValueOrDefault<int>(NumberOfQuakesToShowKey, DefaultSettings.NumberOfQuakesToShowDefaultValue);
            }
            set
            {
                if (AddOrUpdateValue(NumberOfQuakesToShowKey, value))
                {
                    Save();
                }
            }
        }


        /// <summary>
        /// Property to get and set a the Twelve Hour Clock value.
        /// </summary>
        public bool TwentyFourHourClockSetting
        {
            get
            {
                return GetValueOrDefault<bool>(TwentyFourHourClockKey, DefaultSettings.TwentyFourHourClockDefaultValue);
            }
            set
            {
                if (AddOrUpdateValue(TwentyFourHourClockKey, value))
                {
                    Save();
                }
            }
        }


        /// <summary>
        /// Property to get and set whether to show the Live Tile or not.
        /// </summary>
        public bool ShowLiveTileSetting
        {
            get
            {
                return GetValueOrDefault<bool>(ShowLiveTileKey, DefaultSettings.ShowLiveTileDefaultValue);
            }
            set
            {
                if (AddOrUpdateValue(ShowLiveTileKey, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set whether to use the all quakes endpoint or not.
        /// Defaults to false, meaning use the 'felt' endpoint.
        /// </summary>
        public bool UseGeonetAllQuakesEndpointSetting
        {
            get
            {
                return GetValueOrDefault<bool>(UseGeonetAllQuakesEndpointKey, DefaultSettings.UseGeonetAllQuakesEndpointDefaultValue);
            }
            set
            {
                if (AddOrUpdateValue(UseGeonetAllQuakesEndpointKey, value))
                {
                    Save();
                }
            }
        }
    }
}
