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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatsShakingNZ.Settings;
using System.IO.IsolatedStorage;
using Settings;

namespace WhatsShakingNZ.Tests.SettingsTests
{
    [TestClass]
    public class AppSettingsTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void TestSettingsSaveMinDisplayMagnitude()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            settings.MinimumDisplayMagnitudeSetting = 3.2;


            // New instance to ensure it persists.
            settings = new AppSettings();
            Assert.AreEqual(3.2, settings.MinimumDisplayMagnitudeSetting);
        }

        [TestMethod]
        public void TestSettingsSaveMinWarningMagnitude()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            settings.MinimumWarningMagnitudeSetting = 4.3;
            // New instance to ensure it persists.
            settings = new AppSettings();
            Assert.AreEqual(4.3, settings.MinimumWarningMagnitudeSetting);
        }


        [TestMethod]
        public void TestSettingsSaveNumQuakes()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            settings.NumberOfQuakesToShowSetting = 13;
            // New instance to ensure it persists.
            settings = new AppSettings();
            Assert.AreEqual(13, settings.NumberOfQuakesToShowSetting);
        }

        [TestMethod]
        public void TestSettingsSaveLiveTile()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            settings.ShowLiveTileSetting = true;
            // New instance to ensure it persists.
            settings = new AppSettings();
            Assert.AreEqual(true, settings.ShowLiveTileSetting);
        }

        [TestMethod]
        public void TestSettingsSaveTwentyFourHourClock()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            settings.TwentyFourHourClockSetting = true;
            // New instance to ensure it persists.
            settings = new AppSettings();
            Assert.AreEqual(true, settings.TwentyFourHourClockSetting);
        }

        [TestMethod]
        public void TestSettingsSaveUseAllQuakesEndpoint()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            settings.UseGeonetAllQuakesEndpointSetting = true;
            // New instance to ensure it persists.
            settings = new AppSettings();
            Assert.AreEqual(true, settings.UseGeonetAllQuakesEndpointSetting);
        }

        [TestMethod]
        public void TestDefaultSettingsMinDisplayMagnitude()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            Assert.AreEqual(DefaultSettings.MinimumDisplayMagnitudeDefaultValue, settings.MinimumDisplayMagnitudeSetting);

        }
        [TestMethod]
        public void TestDefaultSettingsMinWarningMagnitude()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            Assert.AreEqual(DefaultSettings.MinimumWarningMagnitudeDefaultValue, settings.MinimumWarningMagnitudeSetting);
        }

        [TestMethod]
        public void TestDefaultSettingsNumQuakes()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            Assert.AreEqual(DefaultSettings.NumberOfQuakesToShowDefaultValue, settings.NumberOfQuakesToShowSetting);
        }

        [TestMethod]
        public void TestDefaultSettingsLiveTile()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            Assert.AreEqual(DefaultSettings.ShowLiveTileDefaultValue, settings.ShowLiveTileSetting);
        }

        [TestMethod]
        public void TestDefaultSettingsTwentyFourHourClock()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            Assert.AreEqual(DefaultSettings.TwentyFourHourClockDefaultValue, settings.TwentyFourHourClockSetting);
        }

        [TestMethod]
        public void TestDefaultSettingsUseAllQuakesEndpoint()
        {
            // Delete all saved settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
            AppSettings settings = new AppSettings();
            Assert.AreEqual(DefaultSettings.UseGeonetAllQuakesEndpointDefaultValue, settings.UseGeonetAllQuakesEndpointSetting);
        }
    }
}
