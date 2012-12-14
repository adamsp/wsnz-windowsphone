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
using System.Text.RegularExpressions;
using WhatsShakingNZ.Localization;
using System.Device.Location;

namespace WhatsShakingNZ.Tests.GeonetHelperTests
{
    [TestClass]
    public class DistanceToolTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void TestBearingIsCorrect()
        {
            // Auckland = new Location(174.763351, -36.848457);
            GeoCoordinate northOfAuckland = new GeoCoordinate(-36.8, 174.763351);
            GeoCoordinate eastOfAuckland = new GeoCoordinate(-36.848457, 174.8);
            GeoCoordinate southOfAuckland = new GeoCoordinate(-36.9, 174.763351);
            GeoCoordinate westOfAuckland = new GeoCoordinate(-36.848457, 174.7);
            GeoCoordinate northEastOfAuckland = new GeoCoordinate(-36.8, 174.8);
            GeoCoordinate southEastOfAuckland = new GeoCoordinate(-36.9, 174.8);
            GeoCoordinate southWestOfAuckland = new GeoCoordinate(-36.9, 174.7);
            GeoCoordinate northWestOfAuckland = new GeoCoordinate(-36.8, 174.7);
            Regex northRx = new Regex(@"\b" + AppResources.North + @"\b");
            Regex eastRx = new Regex(@"\b" + AppResources.East + @"\b");
            Regex southRx = new Regex(@"\b" + AppResources.South + @"\b");
            Regex westRx = new Regex(@"\b" + AppResources.West + @"\b");
            Regex northEastRx = new Regex(@"\b" + AppResources.North + ".?" + AppResources.East + @"\b");
            Regex southEastRx = new Regex(@"\b" + AppResources.South + ".?" + AppResources.East + @"\b");
            Regex southWestRx = new Regex(@"\b" + AppResources.South + ".?" + AppResources.West + @"\b");
            Regex northWestRx = new Regex(@"\b" + AppResources.North + ".?" + AppResources.West + @"\b");

            Assert.IsTrue(northRx.Matches(DistanceTool.GetClosestTown(northOfAuckland)).Count == 1);
            Assert.IsTrue(eastRx.Matches(DistanceTool.GetClosestTown(eastOfAuckland)).Count == 1);
            Assert.IsTrue(southRx.Matches(DistanceTool.GetClosestTown(southOfAuckland)).Count == 1);
            Assert.IsTrue(westRx.Matches(DistanceTool.GetClosestTown(westOfAuckland)).Count == 1);
            Assert.IsTrue(northEastRx.Matches(DistanceTool.GetClosestTown(northEastOfAuckland)).Count == 1);
            Assert.IsTrue(southEastRx.Matches(DistanceTool.GetClosestTown(southEastOfAuckland)).Count == 1);
            Assert.IsTrue(southWestRx.Matches(DistanceTool.GetClosestTown(southWestOfAuckland)).Count == 1);
            Assert.IsTrue(northWestRx.Matches(DistanceTool.GetClosestTown(northWestOfAuckland)).Count == 1);
        }

        [TestMethod]
        public void TestClosestTownIsCorrect()
        {
            /**
             * These can easily be found by going to Google Maps, finding a location
             * that is close to the "closest town" you're looking for, right-clicking
             * on the map and selecting "What's Here?". This will give you the
             * long & lat coordinates in the search bar.
             */
            GeoCoordinate closestToAuckland = new GeoCoordinate(-36.823577, 174.91666);
            GeoCoordinate closestToWhangarei = new GeoCoordinate(-36.036884, 174.692814);
            GeoCoordinate closestToNelson = new GeoCoordinate(-41.031715, 173.649696);
            GeoCoordinate closestToDunedin = new GeoCoordinate(-46.012224, 170.360664);

            Assert.IsTrue(DistanceTool.GetClosestTown(closestToAuckland).Contains("Auckland"));
            Assert.IsTrue(DistanceTool.GetClosestTown(closestToWhangarei).Contains("Whangarei"));
            Assert.IsTrue(DistanceTool.GetClosestTown(closestToNelson).Contains("Nelson"));
            Assert.IsTrue(DistanceTool.GetClosestTown(closestToDunedin).Contains("Dunedin"));
        }

        [TestMethod]
        public void TestDistanceIsCorrect()
        {
            GeoCoordinate closestToAuckland = new GeoCoordinate(-36.823577, 174.91666);
            GeoCoordinate closestToWhangarei = new GeoCoordinate(-36.036884, 174.692814);
            GeoCoordinate closestToNelson = new GeoCoordinate(-41.031715, 173.649696);
            GeoCoordinate closestToDunedin = new GeoCoordinate(-46.012224, 170.360664);

            // 14km (13.92 km)
            Assert.IsTrue(DistanceTool.GetClosestTown(closestToAuckland).Contains("14"));
            // 48.03 km
            Assert.IsTrue(DistanceTool.GetClosestTown(closestToWhangarei).Contains("48"));
            // 40.53 km
            Assert.IsTrue(DistanceTool.GetClosestTown(closestToNelson).Contains("41"));
            // 18.47 km
            Assert.IsTrue(DistanceTool.GetClosestTown(closestToDunedin).Contains("18"));

        }

    }
}
