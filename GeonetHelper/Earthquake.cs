using System;
using System.Collections.Generic;
using WhatsShakingNZ.Localization;
using System.Device.Location;

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

        public GeoCoordinate Location { get; set; }
        public string RelativeLocation
        {
            get
            {
                return DistanceTool.GetClosestTown(Location);
            }
        }
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
        public string Status { get; set; }
    }
}
