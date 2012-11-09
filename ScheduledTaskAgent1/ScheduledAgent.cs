using System.Windows;
using System.Linq;
using Microsoft.Phone.Scheduler;
using System.Windows.Media.Imaging;
using WhatsShakingNZ.GeonetHelper;
using Microsoft.Phone.Shell;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System;
using System.Collections.Generic;

namespace ScheduledTaskAgent1
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;
        private const string TileTitle = "Latest NZ Quakes";
        private string TaskName;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
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
            GeonetAccessor.GetQuakesCompletedEvent += QuakeListener;
            GeonetAccessor.GetQuakes();
        }

        public void QuakeListener(object sender, QuakeEventArgs e)
        {
            StandardTileData NewTileData;
            // Application Tile is always the first Tile, even if it is not pinned to Start
            ShellTile TileToFind = ShellTile.ActiveTiles.First();
            // Following code found here: http://stackoverflow.com/questions/8027812/can-i-update-a-live-tile-in-mango-using-local-data
            if (e != null) // If e is null, we have no data connection
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
                    NewTileData = new StandardTileData
                    {
                        Title = quakes.Count + " in last 24hrs",
                        BackContent = string.Format("latest quake\n{0} mag\n{1} k deep",  latest.FormattedMagnitude, latest.FormattedDepth)
                    };
                }
                else
                {
                    NewTileData = new StandardTileData
                    {
                        Title = TileTitle,
                        BackContent = "No quakes in the last 24 hours!"
                    };
                }
            }
            else
            {
                NewTileData = new StandardTileData
                {
                    Title = TileTitle,
                };

            }
            // Application Tile should always be found
            if (TileToFind != null)
            {
                // update the Application Tile
                TileToFind.Update(NewTileData);
            }
            NotifyComplete();
        }
    }
}