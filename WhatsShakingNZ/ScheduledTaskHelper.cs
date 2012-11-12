using System;
using Microsoft.Phone.Scheduler;

namespace WhatsShakingNZ
{
    public static class ScheduledTaskHelper
    {
        static ScheduledTaskHelper() { }

        /// <summary>
        /// Call this to add the background task to update the live tile.
        /// </summary>
        /// <returns></returns>
        public static bool Add()
        {
            PeriodicTask periodicTask = new PeriodicTask(ShakingHelper.PeriodicTaskName);

            periodicTask.Description = "What's Shaking NZ task for live tile";
            periodicTask.ExpirationTime = System.DateTime.Now.AddDays(14);

            // If the agent is already registered with the system,
            if (ScheduledActionService.Find(periodicTask.Name) != null)
            {
                ScheduledActionService.Remove(ShakingHelper.PeriodicTaskName);
            }

            //only can be called when application is running in foreground
            try
            {
                ScheduledActionService.Add(periodicTask);
#if DEBUG
                ScheduledActionService.LaunchForTest(periodicTask.Name, TimeSpan.FromSeconds(60));
#endif
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Call this to remove the background task for the live tile.
        /// </summary>
        public static void Remove()
        {
            // If the agent is already registered with the system,
            if (ScheduledActionService.Find(ShakingHelper.PeriodicTaskName) != null)
            {
                ScheduledActionService.Remove(ShakingHelper.PeriodicTaskName);
            }
        }

        /// <summary>
        /// Call this to refresh the timeout on the background task.
        /// </summary>
        public static void Update()
        {
            Add();
        }
        
    }
}
