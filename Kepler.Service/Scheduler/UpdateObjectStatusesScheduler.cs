using System.Runtime.CompilerServices;
using System.Timers;
using Kepler.Service.Core;

namespace Kepler.Service.Scheduler
{
    public class UpdateObjectStatusesScheduler : Scheduler
    {
        private static Scheduler _instance;

        public static Scheduler GetScheduler
        {
            get
            {
                _instance = _instance ?? new UpdateObjectStatusesScheduler();
                return _instance;
            }
        }

        protected UpdateObjectStatusesScheduler()
        {
        }

        public override void Invoke()
        {
            UpdateObjectsStatuses(this, null);
        }

        public override void Enable()
        {
            ScheduleTimer = new Timer {Interval = 15000};
            ScheduleTimer.Elapsed += UpdateObjectsStatuses;
            ScheduleTimer.Enabled = true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateObjectsStatuses(object sender, ElapsedEventArgs eventArgs)
        {
            ObjectStatusUpdater.UpdateAllObjectStatusesToActual();
        }
    }
}