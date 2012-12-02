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
using Microsoft.Silverlight.Testing;
using WhatsShakingNZ.GeonetHelper;

namespace WhatsShakingNZ.Tests.GeonetHelperTests
{
    [TestClass]
    public class EarthquakeContainerTests : WorkItemTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod, Asynchronous]
        public void TestRefreshViewsDoesntDownload()
        {
            /* The RefreshViews method is only there so that anything listening
             * for a property change can refresh itself via that method - useful
             * when settings change, for example. */
            EarthquakeContainer container = new EarthquakeContainer();
            var quakes = new System.Collections.ObjectModel.ObservableCollection<Earthquake>();
            container.Quakes = quakes;
            container.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(EarthquakeContainer.QuakesUpdatedEventKey, e.PropertyName);
                EnqueueTestComplete();
            };
            container.RefreshViews();
        }

        [TestMethod, Asynchronous]
        public void TestDownloadNewQuakes()
        {
            // Good JSON :)
            // Magnitude > 5 for all of these so we don't accidentally filter them out with low values in AppSettings.
            string json = "{\"type\":\"FeatureCollection\",\"features\":["
                    + "{\"type\":\"Feature\",\"id\":\"quake.2012p904860\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[172.8091,-43.451538]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904860\",\"origintime\":\"2012-11-30 19:09:43.244000\",\"depth\":10.039062,\"magnitude\":5.2373073,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:30:58.437000\"}},"
                    + "{\"type\":\"Feature\",\"id\":\"quake.2012p904809\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[177.92297,-38.60341]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904809\",\"origintime\":\"2012-11-30 18:42:35.602000\",\"depth\":11.972656,\"magnitude\":5.3260586,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:44:28.653000\"}},"
                    + "{\"type\":\"Feature\",\"id\":\"quake.2012p904425\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[175.57617,-39.639915]},\"geometry_name\":\"origin_geom\",\"properties\":{\"publicid\":\"2012p904425\",\"origintime\":\"2012-11-30 15:17:55.860000\",\"depth\":22.929688,\"magnitude\":5.1758049,\"status\":\"reviewed\",\"agency\":\"WEL(GNS_Primary)\",\"updatetime\":\"2012-11-30 19:41:05.659000\"}}"
                + "],\"crs\":{\"type\":\"EPSG\",\"properties\":{\"code\":\"4326\"}}}";
            var response = new Mocks.HttpWebResponse
            {
                getResponseStream = () => new System.IO.MemoryStream(
                    System.Text.Encoding.UTF8.GetBytes(json))
            };

            var request = new Mocks.HttpWebRequest
            {
                getResponse = () => response
            };
            var factory = new Mocks.HttpWebRequestFactory
            {
                create = _ => request
            };
            EarthquakeContainer container = new EarthquakeContainer(factory);
            container.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(EarthquakeContainer.QuakesUpdatedEventKey, e.PropertyName);
                Assert.AreEqual(3, container.Quakes.Count);
                EnqueueTestComplete();
            };
            container.DownloadNewQuakes();
        }

        [TestMethod, Asynchronous]
        public void TestDownloadNewQuakesNoConnection()
        {
            var request = new Mocks.HttpWebRequest
            {
                getResponse = () => { throw new WebException(); }
            };
            var factory = new Mocks.HttpWebRequestFactory
            {
                create = _ => request
            };
            EarthquakeContainer container = new EarthquakeContainer(factory);
            container.PropertyChanged += (sender, e) => {
                Assert.AreEqual(EarthquakeContainer.QuakesUpdatedEventKey, e.PropertyName);
                Assert.AreEqual(0, container.Quakes.Count);
                EnqueueTestComplete();
            };
            container.DownloadNewQuakes();
        }

    }
}
