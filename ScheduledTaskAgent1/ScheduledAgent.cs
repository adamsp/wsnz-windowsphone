using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Localization;
using HttpWebAdapters;

namespace ScheduledTaskAgent1
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;
        private string TileTitle;
        private string TaskName;
        private GeonetAccessor geonet;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            TileTitle = AppResources.LiveTileDefaultTitle;
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
            geonet = new GeonetAccessor(new HttpWebRequestFactory());
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            TaskName = task.Name;
            geonet.GetQuakesCompletedEvent += QuakeListener;
            geonet.GetQuakes();
        }

        public void QuakeListener(object sender, QuakeEventArgs e)
        {
            FlipTileData newTileData;
            // Following code found here: http://stackoverflow.com/questions/8027812/can-i-update-a-live-tile-in-mango-using-local-data
            if (e != null && e.Status == GeonetSuccessStatus.Success) // If e is null or unsuccesful, we have no data connection
            {
                List<Earthquake> quakes = new List<Earthquake>();
                TimeSpan oneDay = new TimeSpan(24,0,0);
                DateTime yesterday = DateTime.Now.Subtract(oneDay);
                foreach (Earthquake q in e.Quakes)
                {
                    if (DateTime.Compare(q.Date, yesterday) > 0) quakes.Add(q); // Within the last 24 hours.
                }
                if (quakes.Count > 0)
                {
                    Earthquake latest = quakes.First();
                    newTileData = new FlipTileData
                    {
                        Title = String.Format(AppResources.LiveTileTitleFormat, quakes.Count),
                        BackTitle = String.Format(AppResources.LiveTileTitleFormat, quakes.Count),
                        BackContent = String.Format(AppResources.LiveTileBackContentFormat,  latest.FormattedMagnitude, latest.FormattedDepth),
                        WideBackContent = String.Format(AppResources.LiveTileWideBackContentFormat, latest.FormattedMagnitude, latest.FormattedDepth, latest.RelativeLocation),
                    };
                }
                else
                {
                    newTileData = new FlipTileData
                    {
                        Title = TileTitle,
                        BackContent = AppResources.LiveTileBackContentNoQuakes,
                        WideBackContent = AppResources.LiveTileBackContentNoQuakes,
                    };
                }
            }
            else
            {
                newTileData = new FlipTileData
                {
                    Title = TileTitle,
                };
            }
            // Application Tile is always the first Tile, even if it is not pinned to Start
            ShellTile tileToFind = ShellTile.ActiveTiles.First();
            // Application Tile should always be found
            if (tileToFind != null)
            {
                // update the Application Tile
                tileToFind.Update(newTileData);
            }
            NotifyComplete();
        }
    }
}