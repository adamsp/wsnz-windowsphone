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
using WhatsShakingNZ.Localization;
using System.Collections.Generic;

namespace WhatsShakingNZ.GeonetHelper
{
    public class DistanceTool
    {
        /**
         * Default locations - North to South.
         * For the choice of locations, I used a map of NZ on my wall
         * and simply selected all the locations that were in a large font.
         * For the latitude & longitude of the locations, I used Google Maps.
         */
        private static Dictionary<string, Location> locations;
        private static Location Whangarei = new Location(174.323735, -35.725156);
        private static Location Auckland = new Location(174.763351, -36.848457);
        private static Location Tauranga = new Location(176.165149, -37.687798);
        private static Location Hamilton = new Location(175.279268, -37.787009);
        private static Location Whakatane = new Location(176.990813, -37.953419);
        private static Location Rotorua = new Location(176.249759, -38.136875);
        private static Location Gisborne = new Location(178.017648, -38.662354);
        private static Location Taupo = new Location(176.070214, -38.685686);
        private static Location NewPlymouth = new Location(174.075247, -39.055622);
        private static Location Napier = new Location(176.912026, -39.492839);
        private static Location Hastings = new Location(176.839247, -39.639558);
        private static Location Wanganui = new Location(175.047932, -39.930093);
        private static Location PalmerstonNorth = new Location(175.608204, -40.352309);
        private static Location Levin = new Location(175.286181, -40.622243);
        private static Location Masterton = new Location(175.657356, -40.951114);
        private static Location UpperHutt = new Location(175.070785, -41.124415);
        private static Location Porirua = new Location(174.840628, -41.133935);
        private static Location LowerHutt = new Location(174.90805, -41.209163);
        private static Location Wellington = new Location(174.776231, -41.28647);
        private static Location Nelson = new Location(173.284049, -41.270632);
        private static Location Blenheim = new Location(173.961261, -41.513444);
        private static Location Greymouth = new Location(171.210765, -42.450398);
        private static Location Christchurch = new Location(172.636268, -43.532041);
        private static Location Timaru = new Location(171.255005, -44.396999);
        private static Location Queenstown = new Location(168.662643, -45.031176);
        private static Location Dunedin = new Location(170.502812, -45.878764);
        private static Location Invercargill = new Location(168.35376, -46.413177);

        static DistanceTool()
        {
            // Not localising these, as I'm not sure how you'd spell "Whangarei" in German...
            locations = new Dictionary<String, Location>();
            locations.Add("Whangarei", Whangarei);
            locations.Add("Auckland", Auckland);
            locations.Add("Tauranga", Tauranga);
            locations.Add("Hamilton", Hamilton);
            locations.Add("Whakatane", Whakatane);
            locations.Add("Rotorua", Rotorua);
            locations.Add("Gisborne", Gisborne);
            locations.Add("Taupo", Taupo);
            locations.Add("New Plymouth", NewPlymouth);
            locations.Add("Napier", Napier);
            locations.Add("Hastings", Hastings);
            locations.Add("Wanganui", Wanganui);
            locations.Add("Palmerston North", PalmerstonNorth);
            locations.Add("Levin", Levin);
            locations.Add("Masterton", Masterton);
            locations.Add("Upper Hutt", UpperHutt);
            locations.Add("Porirua", Porirua);
            locations.Add("Lower Hutt", LowerHutt);
            locations.Add("Wellington", Wellington);
            locations.Add("Nelson", Nelson);
            locations.Add("Blenheim", Blenheim);
            locations.Add("Greymouth", Greymouth);
            locations.Add("Christchurch", Christchurch);
            locations.Add("Timaru", Timaru);
            locations.Add("Queenstown", Queenstown);
            locations.Add("Dunedin", Dunedin);
            locations.Add("Invercargill", Invercargill);
        }
        public static String GetClosestTown(Location quakeEpicenter)
        {
            // Find the distance from the closest town
            double closestTownDistance = -1;
            String closestTownName = null;
            Location closestTown = null;
            foreach (var location in locations.Keys)
            {
                if (closestTownDistance < 0)
                {
                    closestTownDistance = DistanceBetweenPlaces(quakeEpicenter.Longitude, quakeEpicenter.Latitude, locations[location].Longitude, locations[location].Latitude);
                    closestTownName = location;
                    closestTown = locations[location];
                }
                else
                {
                    double distance = DistanceBetweenPlaces(quakeEpicenter.Longitude, quakeEpicenter.Latitude, locations[location].Longitude, locations[location].Latitude);
                    if (distance < closestTownDistance)
                    {
                        closestTownDistance = distance;
                        closestTownName = location;
                        closestTown = locations[location];
                    }
                }
            }

            // Find direction from the closest town
            String direction = GetDirectionFromTown(closestTown, quakeEpicenter);

            String locationFormatString = AppResources.EarthquakeLocationFormat;
            return String.Format(locationFormatString, closestTownDistance, direction, closestTownName);
        }

        private static String GetDirectionFromTown(Location closestTown,
                Location quakeEpicenter)
        {
            double dLon = Math.Abs(quakeEpicenter.Longitude - closestTown.Longitude);
            double dLat = Math.Abs(quakeEpicenter.Latitude - closestTown.Latitude);
            double brng = Math.Atan(dLat / dLon);

            String direction;
            String eastOrWest;
            String northOrSouth;
            // Quake is West of town
            if (quakeEpicenter.Longitude < closestTown.Longitude)
                eastOrWest = AppResources.West;
            else // Quake is East of town
                eastOrWest = AppResources.East;

            // Quake is North of town
            if (quakeEpicenter.Latitude > closestTown.Latitude)
                northOrSouth = AppResources.North;
            else // Quake is South of town
                northOrSouth = AppResources.South;

            if (brng < Math.PI / 8)
                direction = eastOrWest;
            else if (brng < 3 * Math.PI / 8)
                direction = String.Format(AppResources.NorthEastFormat, northOrSouth, eastOrWest);
            else
                direction = northOrSouth;

            return direction;
        }

        
        #region DistanceAlgorithm
        // As seen here: http://stackoverflow.com/questions/27928/how-do-i-calculate-distance-between-two-latitude-longitude-points
        const double PIx = 3.141592653589793;
        const double RADIO = 6378.16; // Radius of the earth, in km

        /// <summary>
        /// Convert degrees to Radians
        /// </summary>
        /// <param name="x">Degrees</param>
        /// <returns>The equivalent in radians</returns>
        public static double Radians(double x)
        {
            return x * PIx / 180;
        }

        /// <summary>
        /// Calculate the distance between two places.
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double DistanceBetweenPlaces(
            double lon1,
            double lat1,
            double lon2,
            double lat2)
        {
            double dlon = Radians(lon2 - lon1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * RADIO;
        }
        #endregion DistanceAlgorithm
    }
}
