using System;

namespace WhatsShakingNZ.GeonetHelper
{
    public class Earthquake
    {
        private double _depth;
        public double Depth
        {
            get
            {
                return _depth;
            }
            set
            {
                _depth = Math.Round(value, 0);
            }
        }

        /// <summary>
        /// A string representation of this Earthquakes depth, rounded
        /// as per http://info.geonet.org.nz/display/appdata/Earthquake+Web+Services
        /// </summary>
        public string FormattedDepth
        {
            get
            {
                return string.Format("{0:0}", Depth);
            }
        }

        public Location Location { get; set; }
        private double _magnitude;
        public double Magnitude
        {
            get
            {
                return _magnitude;
            }
            set
            {
                _magnitude = Math.Round(value, 1);
            }
        }

        /// <summary>
        /// A string representation of this Earthquakes magnitude, rounded
        /// as per http://info.geonet.org.nz/display/appdata/Earthquake+Web+Services
        /// </summary>
        public string FormattedMagnitude
        {
            get
            {
                return string.Format("{0:N1}", Magnitude);
            }
        }
        public string Reference { get; set; }
        public string Agency { get; set; }
        public DateTime Date { get; set; }

        /// <summary>
        /// Returns the distance this Quake occurred from the provided location, in kilometers.
        /// </summary>
        /// <param name="location">The location you want to measure from.</param>
        /// <returns>The distance from your provided location to this quake, in kilometers.</returns>
        public double Distance(Location location)
        {
            return DistanceAlgorithm.DistanceBetweenPlaces(Location.Longitude, Location.Latitude, location.Longitude, location.Latitude);
        }
    }

    public struct Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    /// <summary>
    /// As seen here: http://stackoverflow.com/questions/27928/how-do-i-calculate-distance-between-two-latitude-longitude-points
    /// </summary>
    class DistanceAlgorithm
    {
        const double PIx = 3.141592653589793;
        const double RADIO = 6378.16; // Radius of the earth, in km

        /// <summary>
        /// This class cannot be instantiated.
        /// </summary>
        private DistanceAlgorithm() { }

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
    }
}
