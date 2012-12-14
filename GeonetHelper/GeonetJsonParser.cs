using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Device.Location;
using Newtonsoft.Json;

namespace WhatsShakingNZ.GeonetHelper
{
    public class GeonetJsonParser
    {
        public IEnumerable<Earthquake> ParseJsonToQuakes(string json)
        {
            try
            {
                JObject o = JObject.Parse(json);
                List<Earthquake> quakes = new List<Earthquake>(30);
                foreach (var q in o["features"].Children())
                {
                    Earthquake quake = new Earthquake
                    {
                        Location = new GeoCoordinate(q["geometry"]["coordinates"].Values<double>().ElementAt(1),
                            q["geometry"]["coordinates"].Values<double>().ElementAt(0)),
                        Depth = (double)q["properties"]["depth"],
                        Magnitude = (double)q["properties"]["magnitude"],
                        Reference = (string)q["properties"]["publicid"],
                        /* origintime=2012-08-13 05:25:24.727000  (that's in UTC) */
                        Date = DateTime.Parse((string)q["properties"]["origintime"] + "Z"),
                        Agency = (string)q["properties"]["agency"],
                        Status = (string)q["properties"]["status"]
                    };
                    quakes.Add(quake);
                }
                return quakes.OrderByDescending(q => q.Date);
            }
            catch (Exception e)
            {
                throw new JsonException("Problem with JSON data", e);
            }
        }
    }
}
