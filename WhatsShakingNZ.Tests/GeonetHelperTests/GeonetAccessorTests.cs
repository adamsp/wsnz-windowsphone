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
using WhatsShakingNZ.Settings;

namespace WhatsShakingNZ.Tests.GeonetHelperTests
{
    [TestClass]
    public class GeonetAccessorTests : WorkItemTest
    {
        // Good JSON :)
        private const string goodJson = "{\"type\":\"FeatureCollection\",\"features\":["
                    + "{\"type\":\"Feature\",\"id\":\"quake.2012p904860\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[172.8091,-43.451538]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904860\",\"origintime\":\"2012-11-30 19:09:43.244000\",\"depth\":10.039062,\"magnitude\":3.2373073,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:30:58.437000\"}},"
                    + "{\"type\":\"Feature\",\"id\":\"quake.2012p904809\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[177.92297,-38.60341]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904809\",\"origintime\":\"2012-11-30 18:42:35.602000\",\"depth\":11.972656,\"magnitude\":2.3260586,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:44:28.653000\"}},"
                    + "{\"type\":\"Feature\",\"id\":\"quake.2012p904425\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[175.57617,-39.639915]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904425\",\"origintime\":\"2012-11-30 15:17:55.860000\",\"depth\":22.929688,\"magnitude\":3.1758049,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:41:05.659000\"}}"
                + "],\"crs\":{\"type\":\"EPSG\",\"properties\":{\"code\":\"4326\"}}}";

        // Incomplete JSON
        string badJson = "{\"type\":\"FeatureCollection\",\"features\":[{\"type\":\"Feature\",\"id\":\"quake.2012p904860\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[172.8091,-43.451538]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904860\",\"origintime\":\"2012-11-30 19:09:43.244000\",\"depth\":10.039062,\"magnitude\":3.2373073,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:30:58.437000\"}}";

        // Empty Json back from Geonet
        string emptyJson = "{}";

        // Empty text back from Geonet
        string noDataJson = string.Empty;

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
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(badJson))
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
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(noDataJson))
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
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(emptyJson))
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
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(goodJson))
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

        [TestMethod, Asynchronous]
        public void TestGetQuakesUsesCorrectUrl()
        {
            AppSettings settings = new AppSettings();
            
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(goodJson))
            };
            var request = new Mocks.HttpWebRequest
            {
                getResponse = () => response
            };

            // 'felt' quakes is used
            settings.UseGeonetAllQuakesEndpointSetting = false;
            GeonetAccessor geonet = new GeonetAccessor(new Mocks.HttpWebRequestFactory
            {
                create = (url) => {
                    Assert.AreEqual(GeonetEndpoints.FeltQuakes, url.AbsoluteUri);
                    return request;
                }
            });
            geonet.GetQuakes();

            // 'all' quakes is used
            settings.UseGeonetAllQuakesEndpointSetting = true;
            geonet = new GeonetAccessor(new Mocks.HttpWebRequestFactory
            {
                create = (url) =>
                {
                    Assert.AreEqual(GeonetEndpoints.AllQuakes, url.AbsoluteUri);
                    EnqueueTestComplete();
                    return request;
                }
            });
            geonet.GetQuakes();
        }
    }
}
