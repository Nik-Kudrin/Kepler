using System.Timers;

namespace Kepler.Service.Scheduler
{
    public abstract class Scheduler
    {
        protected Timer ScheduleTimer;

        protected Scheduler()
        {
        }

        public abstract void Invoke();
        public abstract void Enable();
    }
}