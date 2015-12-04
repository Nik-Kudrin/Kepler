using System;
using System.Runtime.CompilerServices;
using System.Timers;
using Kepler.Common.CommunicationContracts;

namespace Kepler.Service.Scheduler
{
    public class CleanDataSchedulerBase : Scheduler
    {
        private string _schedulerName;
        protected DataSchedulerContract SchedulerInfo;

        protected CleanDataSchedulerBase(string schedulerName)
        {
            _schedulerName = schedulerName;
            UpdateScheduleInfo();
        }

        private void UpdateScheduleInfo()
        {
            SchedulerInfo = new KeplerService().GetCleanDataScheduler(_schedulerName);
        }

        public override void Invoke()
        {
            CleanData(null, null);
        }

        public override void Enable()
        {
            ScheduleTimer = new Timer {Interval = 600000}; // every 10 min
            ScheduleTimer.Elapsed += CleanData;
            ScheduleTimer.Enabled = true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected virtual void CleanData(object sender, ElapsedEventArgs eventArgs)
        {
            UpdateScheduleInfo(); // Get latest actual schedule info

            var now = DateTime.Now;
            if (now < SchedulerInfo.NextStartTime.Value) return;

            SchedulerInfo.LastStartTime = now; // because LastStart time may be null
            SchedulerInfo.NextStartTime = SchedulerInfo.LastStartTime + SchedulerInfo.SchedulePeriod;
            new KeplerService().UpdateCleanDataScheduler(SchedulerInfo);
        }
    }
}