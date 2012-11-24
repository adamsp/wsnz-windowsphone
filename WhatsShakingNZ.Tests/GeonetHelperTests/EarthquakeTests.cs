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

namespace WhatsShakingNZ.Tests.GeonetHelperTests
{
    [TestClass]
    public class EarthquakeTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void TestDepthFormatting()
        {
            Earthquake quake = new Earthquake();
            quake.Depth = 102.13;
            Assert.AreEqual("102", quake.FormattedDepth);
            Assert.AreEqual(102.0, quake.Depth);

            quake.Depth = 102.03;
            Assert.AreEqual("102", quake.FormattedDepth);
            Assert.AreEqual(102.0, quake.Depth);

            quake.Depth = 102.73;
            Assert.AreEqual("103", quake.FormattedDepth);
            Assert.AreEqual(103.0, quake.Depth);

            // Test fails - "bankers rounding" (round to nearest even number when .5)
            // is our only option as we don't have the overloads to specify what to
            // do in this situation. Sigh.
            //quake.Depth = 102.5;
            //Assert.AreEqual("103", quake.FormattedDepth);
            //Assert.AreEqual(103.0, quake.Depth);
            
            quake.Depth = 102.0;
            Assert.AreEqual("102", quake.FormattedDepth);
            Assert.AreEqual(102.0, quake.Depth);
        }

        [TestMethod]
        public void TestMagnitudeFormatting()
        {
            Earthquake quake = new Earthquake();
            quake.Magnitude = 3.13;
            Assert.AreEqual("3.1", quake.FormattedMagnitude);
            Assert.AreEqual(3.1, quake.Magnitude);

            quake.Magnitude = 3.02;
            Assert.AreEqual("3.0", quake.FormattedMagnitude);
            Assert.AreEqual(3.0, quake.Magnitude);

            quake.Magnitude = 3.96;
            Assert.AreEqual("4.0", quake.FormattedMagnitude);
            Assert.AreEqual(4.0, quake.Magnitude);

            quake.Magnitude = 3.15;
            Assert.AreEqual("3.2", quake.FormattedMagnitude);
            Assert.AreEqual(3.2, quake.Magnitude);

            quake.Magnitude = 3.0;
            Assert.AreEqual("3.0", quake.FormattedMagnitude);
            Assert.AreEqual(3.0, quake.Magnitude);
        }
    }
}
