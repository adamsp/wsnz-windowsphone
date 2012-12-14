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
using System.Device.Location;

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
        private static Dictionary<string, GeoCoordinate> locations;
        private static GeoCoordinate Whangarei = new GeoCoordinate(-35.725156, 174.323735);
        private static GeoCoordinate Auckland = new GeoCoordinate(-36.848457, 174.763351);
        private static GeoCoordinate Tauranga = new GeoCoordinate(-37.687798, 176.165149);
        private static GeoCoordinate Hamilton = new GeoCoordinate(-37.787009, 175.279268);
        private static GeoCoordinate Whakatane = new GeoCoordinate(-37.953419, 176.990813);
        private static GeoCoordinate Rotorua = new GeoCoordinate(-38.136875, 176.249759);
        private static GeoCoordinate Gisborne = new GeoCoordinate(-38.662354, 178.017648);
        private static GeoCoordinate Taupo = new GeoCoordinate(-38.685686, 176.070214);
        private static GeoCoordinate NewPlymouth = new GeoCoordinate(-39.055622, 174.075247);
        private static GeoCoordinate Napier = new GeoCoordinate(-39.492839, 176.912026);
        private static GeoCoordinate Hastings = new GeoCoordinate(-39.639558, 176.839247);
        private static GeoCoordinate Wanganui = new GeoCoordinate(-39.930093, 175.047932);
        private static GeoCoordinate PalmerstonNorth = new GeoCoordinate(-40.352309, 175.608204);
        private static GeoCoordinate Levin = new GeoCoordinate(-40.622243, 175.286181);
        private static GeoCoordinate Masterton = new GeoCoordinate(-40.951114, 175.657356);
        private static GeoCoordinate UpperHutt = new GeoCoordinate(-41.124415, 175.070785);
        private static GeoCoordinate Porirua = new GeoCoordinate(-41.133935, 174.840628);
        private static GeoCoordinate LowerHutt = new GeoCoordinate(-41.209163, 174.90805);
        private static GeoCoordinate Wellington = new GeoCoordinate(-41.28647, 174.776231);
        private static GeoCoordinate Nelson = new GeoCoordinate(-41.270632, 173.284049);
        private static GeoCoordinate Blenheim = new GeoCoordinate(-41.513444, 173.961261);
        private static GeoCoordinate Greymouth = new GeoCoordinate(-42.450398, 171.210765);
        private static GeoCoordinate Christchurch = new GeoCoordinate(-43.532041, 172.636268);
        private static GeoCoordinate Timaru = new GeoCoordinate(-44.396999, 171.255005);
        private static GeoCoordinate Queenstown = new GeoCoordinate(-45.031176, 168.662643);
        private static GeoCoordinate Dunedin = new GeoCoordinate(-45.878764, 170.502812);
        private static GeoCoordinate Invercargill = new GeoCoordinate(-46.413177, 168.35376);

        static DistanceTool()
        {
            // Not localising these, as I'm not sure how you'd spell "Whangarei" in German...
            locations = new Dictionary<String, GeoCoordinate>();
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
        public static String GetClosestTown(GeoCoordinate quakeEpicenter)
        {
            // Find the distance from the closest town
            double closestTownDistance = -1;
            String closestTownName = null;
            GeoCoordinate closestTown = null;
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

        private static String GetDirectionFromTown(GeoCoordinate closestTown,
                GeoCoordinate quakeEpicenter)
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
        private static double Radians(double x)
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
        private static double DistanceBetweenPlaces(
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
