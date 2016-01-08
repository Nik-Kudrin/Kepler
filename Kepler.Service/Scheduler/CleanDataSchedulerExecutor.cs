namespace Kepler.Service.Scheduler
{
    public class CleanDataSchedulerExecutor
    {
        private static CleanDataSchedulerExecutor _executor;
        private static BuildCleanDataScheduler _buildCleanScheduler;
        private static LogCleanDataScheduler _logCleanScheduler;

        static CleanDataSchedulerExecutor()
        {
            GetExecutor();
        }

        private CleanDataSchedulerExecutor()
        {
            _buildCleanScheduler = new BuildCleanDataScheduler();
            _buildCleanScheduler.Enable();
            _logCleanScheduler = new LogCleanDataScheduler();
            _logCleanScheduler.Enable();
        }

        public static CleanDataSchedulerExecutor GetExecutor()
        {
            _executor = _executor ?? new CleanDataSchedulerExecutor();
            return _executor;
        }
    }
}