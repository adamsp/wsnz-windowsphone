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
using Newtonsoft.Json;
using WhatsShakingNZ.Settings;

namespace WhatsShakingNZ.GeonetHelper
{
    public enum GeonetSuccessStatus { Success = 0, NetworkFailure, BadGeonetData, NoGeonetData }

    public class GeonetEndpoints
    {
        public const string AllQuakes = "http://geonet.org.nz/quakes/services/all.json";
        public const string FeltQuakes = "http://geonet.org.nz/quakes/services/felt.json";
    }

    public class GeonetAccessor
    {
        public QuakeEventHandler GetQuakesCompletedEvent;

        private IHttpWebRequestFactory webRequestFactory;

        private AppSettings settings;

        public GeonetAccessor(IHttpWebRequestFactory webRequestFactory)
        {
            this.webRequestFactory = webRequestFactory;
            this.settings = new AppSettings();
        }

        public void GetQuakes()
        {
            string endpoint;
            if (settings.UseGeonetAllQuakesEndpointSetting)
                endpoint = GeonetEndpoints.AllQuakes;
            else
                endpoint = GeonetEndpoints.FeltQuakes;
            GetQuakes(endpoint);
        }

        private void GetQuakes(string uriString)
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
                catch (WebException)
                {
                    Completed(null, GeonetSuccessStatus.NetworkFailure);
                    return;
                }
            }
            IEnumerable<Earthquake> quakes;
            if (!string.IsNullOrEmpty(responseJson))
            {

                try
                {
                    GeonetJsonParser jsonParser = new GeonetJsonParser();
                    quakes = jsonParser.ParseJsonToQuakes(responseJson);
                    Completed(quakes, GeonetSuccessStatus.Success);
                }
                catch (JsonException e)
                {
                    Completed(null, GeonetSuccessStatus.BadGeonetData);
                }
            }
            else
            {
                Completed(null, GeonetSuccessStatus.NoGeonetData);
            }
        }

        private void Completed(IEnumerable<Earthquake> quakes, GeonetSuccessStatus success)
        {
            if (null != GetQuakesCompletedEvent)
                GetQuakesCompletedEvent(this, new QuakeEventArgs(quakes, success));
        }
    }

    public delegate void QuakeEventHandler(object sender, QuakeEventArgs e);
    public class QuakeEventArgs : EventArgs
    {
        private ObservableCollection<Earthquake> _quakes;
        private GeonetSuccessStatus _status;
        public QuakeEventArgs(IEnumerable<Earthquake> quakes, GeonetSuccessStatus status)
        {
            if (quakes != null)
                _quakes = new ObservableCollection<Earthquake>(quakes);
            else
                _quakes = new ObservableCollection<Earthquake>();
            _status = status;
        }
        public GeonetSuccessStatus Status
        {
            get
            {
                return _status;
            }
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
