using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Device.Location;
using HttpWebAdapters;
using HttpWebAdapters.Adapters;

namespace WhatsShakingNZ.GeonetHelper
{
    public class GeonetAccessor
    {
        public QuakeEventHandler GetQuakesCompletedEvent;

        private IHttpWebRequestFactory webRequestFactory;

        public GeonetAccessor(IHttpWebRequestFactory webRequestFactory)
        {
            this.webRequestFactory = webRequestFactory;
        }

        public void GetQuakes()
        {
            GetQuakes("http://geonet.org.nz/quakes/services/felt.json");
        }

        public void GetQuakes(string uriString)
        {
            var request = webRequestFactory.Create(new Uri(uriString));
            request.AllowReadStreamBuffering = true;
            var token = request.BeginGetResponse(ProcessResponse, request);
        }

        void ProcessResponse(IAsyncResult result)
        {
            IHttpWebRequest request = result.AsyncState as IHttpWebRequest;
            string responseJson = "";
            if (request != null)
            {
                try
                {
                    var response = request.EndGetResponse(result);
                    using (var stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            responseJson = reader.ReadToEnd();
                        }
                    }
                }
                catch (WebException e)
                {
                    if(null != GetQuakesCompletedEvent)
                        GetQuakesCompletedEvent(null, null);
                    return;
                }
            }
            List<Earthquake> quakes = new List<Earthquake>();
            if (!string.IsNullOrEmpty(responseJson))
            {
                
                try
                {
                    JObject o = JObject.Parse(responseJson);
                    
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
                    quakes = new List<Earthquake>(quakes.OrderByDescending(q => q.Date));
                }
                catch {}
            }
            if(null != GetQuakesCompletedEvent)
                GetQuakesCompletedEvent(null, new QuakeEventArgs(quakes));
        }
    }
    public delegate void QuakeEventHandler(object sender, QuakeEventArgs e);
    public class QuakeEventArgs : EventArgs
    {
        private ObservableCollection<Earthquake> _quakes;
        public QuakeEventArgs(List<Earthquake> quakes)
        {
            _quakes = new ObservableCollection<Earthquake>(quakes);
        }
        public ObservableCollection<Earthquake> Quakes
        {
            get
            {
                return _quakes;
            }
        }
    }
}
