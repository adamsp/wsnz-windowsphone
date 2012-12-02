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
        public void TestSettingsSave()
        {
            AppSettings settings = new AppSettings();
            settings.MinimumDisplayMagnitudeSetting = 3.2;
            settings.MinimumWarningMagnitudeSetting = 4.3;
            settings.NumberOfQuakesToShowSetting = 13;
            settings.ShowLiveTileSetting = true;
            settings.TwentyFourHourClockSetting = false;

            // New instance to ensure it persists.
            settings = new AppSettings();
            Assert.AreEqual(3.2, settings.MinimumDisplayMagnitudeSetting);
            Assert.AreEqual(4.3, settings.MinimumWarningMagnitudeSetting);
            Assert.AreEqual(13, settings.NumberOfQuakesToShowSetting);
            Assert.AreEqual(true, settings.ShowLiveTileSetting);
            Assert.AreEqual(false, settings.TwentyFourHourClockSetting);
        }
    }
}
