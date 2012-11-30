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
using HttpWebAdapters;
using Microsoft.Silverlight.Testing;

namespace WhatsShakingNZ.Tests.GeonetHelperTests
{
    [TestClass]
    public class GeonetAccessorTests : WorkItemTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod, Asynchronous]
        public void TestGetQuakesHandlesNoConnection()
        {
            var request = new Mocks.HttpWebRequest
            {
                getResponse = () => { throw new WebException(); }
            };
            GeonetAccessor geonet = new GeonetAccessor(new Mocks.HttpWebRequestFactory
            {
                create = _ => request
            });
            geonet.GetQuakesCompletedEvent += (sender, e) => {
                Assert.AreEqual(geonet, sender);
                Assert.AreEqual(GeonetSuccessStatus.NetworkFailure, e.Status);
                Assert.AreEqual(0, e.Quakes.Count);
                EnqueueTestComplete();
            };
            geonet.GetQuakes();
        }

        [TestMethod, Asynchronous]
        public void TestGetQuakesHandlesBadJson()
        {
            // Incomplete JSON
            string json = "{\"type\":\"FeatureCollection\",\"features\":[{\"type\":\"Feature\",\"id\":\"quake.2012p904860\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[172.8091,-43.451538]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904860\",\"origintime\":\"2012-11-30 19:09:43.244000\",\"depth\":10.039062,\"magnitude\":3.2373073,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:30:58.437000\"}}";
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(json))
            };
            var request = new Mocks.HttpWebRequest
            {
                getResponse = () => response
            };
            GeonetAccessor geonet = new GeonetAccessor(new Mocks.HttpWebRequestFactory
            {
                create = _ => request
            });
            geonet.GetQuakesCompletedEvent += (sender, e) =>
            {
                Assert.AreEqual(geonet, sender);
                Assert.AreEqual(GeonetSuccessStatus.BadGeonetData, e.Status);
                Assert.AreEqual(0, e.Quakes.Count);
                EnqueueTestComplete();
            };
            geonet.GetQuakes();
        }

        [TestMethod, Asynchronous]
        public void TestGetQuakesHandlesNoData()
        {
            // Empty text back from Geonet
            string json = string.Empty;
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(json))
            };
            var request = new Mocks.HttpWebRequest
            {
                getResponse = () => response
            };
            GeonetAccessor geonet = new GeonetAccessor(new Mocks.HttpWebRequestFactory
            {
                create = _ => request
            });
            geonet.GetQuakesCompletedEvent += (sender, e) =>
            {
                Assert.AreEqual(geonet, sender);
                Assert.AreEqual(GeonetSuccessStatus.NoGeonetData, e.Status);
                Assert.AreEqual(0, e.Quakes.Count);
                EnqueueTestComplete();
            };
            geonet.GetQuakes();
        }

        [TestMethod, Asynchronous]
        public void TestGetQuakesHandlesEmptyJson()
        {
            // Empty Json back from Geonet
            string json = "{}";
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(json))
            };
            var request = new Mocks.HttpWebRequest
            {
                getResponse = () => response
            };
            GeonetAccessor geonet = new GeonetAccessor(new Mocks.HttpWebRequestFactory
            {
                create = _ => request
            });
            geonet.GetQuakesCompletedEvent += (sender, e) =>
            {
                Assert.AreEqual(geonet, sender);
                Assert.AreEqual(GeonetSuccessStatus.BadGeonetData, e.Status);
                Assert.AreEqual(0, e.Quakes.Count);
                EnqueueTestComplete();
            };
            geonet.GetQuakes();
        }

        [TestMethod, Asynchronous]
        public void TestGetQuakesHandlesGoodJson()
        {
            // Good JSON :)
            string json = "{\"type\":\"FeatureCollection\",\"features\":["
                    +"{\"type\":\"Feature\",\"id\":\"quake.2012p904860\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[172.8091,-43.451538]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904860\",\"origintime\":\"2012-11-30 19:09:43.244000\",\"depth\":10.039062,\"magnitude\":3.2373073,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:30:58.437000\"}},"
                    +"{\"type\":\"Feature\",\"id\":\"quake.2012p904809\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[177.92297,-38.60341]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904809\",\"origintime\":\"2012-11-30 18:42:35.602000\",\"depth\":11.972656,\"magnitude\":2.3260586,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:44:28.653000\"}},"
                    +"{\"type\":\"Feature\",\"id\":\"quake.2012p904425\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[175.57617,-39.639915]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904425\",\"origintime\":\"2012-11-30 15:17:55.860000\",\"depth\":22.929688,\"magnitude\":3.1758049,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:41:05.659000\"}}"
                +"],\"crs\":{\"type\":\"EPSG\",\"properties\":{\"code\":\"4326\"}}}";
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(json))
            };
            var request = new Mocks.HttpWebRequest
            {
                getResponse = () => response
            };
            GeonetAccessor geonet = new GeonetAccessor(new Mocks.HttpWebRequestFactory
            {
                create = _ => request
            });
            geonet.GetQuakesCompletedEvent += (sender, e) =>
            {
                Assert.AreEqual(geonet, sender);
                Assert.AreEqual(GeonetSuccessStatus.Success, e.Status);
                Assert.AreEqual(3, e.Quakes.Count);
                EnqueueTestComplete();
            };
            geonet.GetQuakes();
        }
    }
}
