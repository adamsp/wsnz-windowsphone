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
using System.Device.Location;
using Newtonsoft.Json;

namespace WhatsShakingNZ.Tests.GeonetHelperTests
{
    [TestClass]
    public class GeonetJsonParserTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void TestJsonIsParsedCorrectly()
        {
            // Good JSON :)
            string json = "{\"type\":\"FeatureCollection\",\"features\":["
                    + "{\"type\":\"Feature\",\"id\":\"quake.2012p904860\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[172.8091,-43.451538]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904860\",\"origintime\":\"2012-11-30 19:09:43.244000\",\"depth\":10.039062,\"magnitude\":3.2373073,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:30:58.437000\"}},"
                    + "{\"type\":\"Feature\",\"id\":\"quake.2012p904809\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[177.92297,-38.60341]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904809\",\"origintime\":\"2012-11-30 18:42:35.602000\",\"depth\":11.972656,\"magnitude\":2.3260586,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:44:28.653000\"}},"
                    + "{\"type\":\"Feature\",\"id\":\"quake.2012p904425\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[175.57617,-39.639915]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904425\",\"origintime\":\"2012-11-30 15:17:55.860000\",\"depth\":22.929688,\"magnitude\":3.1758049,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:41:05.659000\"}}"
                + "],\"crs\":{\"type\":\"EPSG\",\"properties\":{\"code\":\"4326\"}}}";
            GeonetJsonParser parser = new GeonetJsonParser();
            var quakeCollection = parser.ParseJsonToQuakes(json);
            var quakes = new List<Earthquake>(quakeCollection);
            Assert.AreEqual(3, quakes.Count);
            var quake = quakes[0];
            /*
                * "{\"type\":\"Feature\",\"id\":\"quake.2012p904860\",\"geometry\":
             * {\"type\":\"Point\",
             * \"coordinates\":[172.8091,-43.451538]},
             * \"geometry_name\":\"origin_geom\",
             * \"properties\":
             * {\"publicid\":\"2012p904860\",
             * \"origintime\":\"2012-11-30 19:09:43.244000\",
             * \"depth\":10.039062,
             * \"magnitude\":3.2373073,
             * \"status\":\"reviewed\",
             * \"agency\":\"WEL(GNS_Primary)\",
             * \"updatetime\":\"2012-11-30 19:30:58.437000\"}},"*/
            Assert.AreEqual("WEL(GNS_Primary)", quake.Agency);
            Assert.AreEqual(new DateTime(2012, 11, 30, 19, 09, 43, 244, DateTimeKind.Utc), quake.Date.ToUniversalTime());
            Assert.AreEqual(10.0, quake.Depth);
            Assert.AreEqual(new GeoCoordinate(-43.451538, 172.8091).Latitude, quake.Location.Latitude);
            Assert.AreEqual(new GeoCoordinate(-43.451538, 172.8091).Longitude, quake.Location.Longitude);
            Assert.AreEqual(3.2, quake.Magnitude);
            Assert.AreEqual("2012p904860", quake.Reference);
            Assert.AreEqual("reviewed", quake.Status);
        }

        [TestMethod]
        public void TestBadJsonThrowsJsonException()
        {
            // Incomplete JSON
            string json = "{\"type\":\"FeatureCollection\",\"features\":[{\"type\":\"Feature\",\"id\":\"quake.2012p904860\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[172.8091,-43.451538]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904860\",\"origintime\":\"2012-11-30 19:09:43.244000\",\"depth\":10.039062,\"magnitude\":3.2373073,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:30:58.437000\"}}";
            GeonetJsonParser parser = new GeonetJsonParser();
            try
            {
                var quakes = parser.ParseJsonToQuakes(json);
            }
            catch (JsonException e)
            {
                // We're expecting a JsonException, so this is good.
            }
            catch (Exception e)
            {
                // We want to fail on any other kind of exception.
                Assert.Fail();
            }
        }
    }
}
