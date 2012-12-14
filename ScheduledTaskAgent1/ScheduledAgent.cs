using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Localization;
using HttpWebAdapters;
using TileControls;

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

            FlipTileController ftc = new FlipTileController();
            if (e != null && e.Status == GeonetSuccessStatus.Success) // If e is null or unsuccesful, we have no data connection
            {
                ftc.UpdateFlipTile(e.Quakes);
            }
            else
            {
                ftc.ClearFlipTile();
            }
            NotifyComplete();
        }
    }
}