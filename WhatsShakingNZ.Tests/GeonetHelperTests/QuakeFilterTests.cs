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
using WhatsShakingNZ.GeonetHelper;
using System.Collections.Generic;
using WhatsShakingNZ.Settings;

namespace WhatsShakingNZ.Tests.GeonetHelperTests
{
    [TestClass]
    public class QuakeFilterTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void TestFilteringSucceeds()
        {
            List<Earthquake> quakes = new List<Earthquake>();
            quakes.Add(new Earthquake { Magnitude = 1 });
            quakes.Add(new Earthquake { Magnitude = 1 });
            quakes.Add(new Earthquake { Magnitude = 2 });
            quakes.Add(new Earthquake { Magnitude = 2 });
            quakes.Add(new Earthquake { Magnitude = 2 });
            quakes.Add(new Earthquake { Magnitude = 3 });
            quakes.Add(new Earthquake { Magnitude = 3 });
            quakes.Add(new Earthquake { Magnitude = 3 });
            quakes.Add(new Earthquake { Magnitude = 3 });
            quakes.Add(new Earthquake { Magnitude = 3 });
            quakes.Add(new Earthquake { Magnitude = 3 });
            quakes.Add(new Earthquake { Magnitude = 3 });
            quakes.Add(new Earthquake { Magnitude = 3 });
            quakes.Add(new Earthquake { Magnitude = 4 });
            quakes.Add(new Earthquake { Magnitude = 4 });
            quakes.Add(new Earthquake { Magnitude = 4 });
            quakes.Add(new Earthquake { Magnitude = 4 });
            quakes.Add(new Earthquake { Magnitude = 4 });
            quakes.Add(new Earthquake { Magnitude = 4 });
            quakes.Add(new Earthquake { Magnitude = 4 });
            AppSettings settings = new AppSettings();
            settings.MinimumDisplayMagnitudeSetting = 3;
            settings.NumberOfQuakesToShowSetting = 10;
            var filteredQuakes = QuakeFilter.GetFilteredQuakes(quakes, settings);

            Assert.IsTrue(filteredQuakes.Count == 10);
            
            foreach (var quake in filteredQuakes)
                Assert.IsTrue(quake.Magnitude >= 3);

            settings.NumberOfQuakesToShowSetting = 30;
            filteredQuakes = QuakeFilter.GetFilteredQuakes(quakes, settings);

            Assert.IsTrue(filteredQuakes.Count == 15);

            foreach (var quake in filteredQuakes)
                Assert.IsTrue(quake.Magnitude >= 3);
        }
    }
}
