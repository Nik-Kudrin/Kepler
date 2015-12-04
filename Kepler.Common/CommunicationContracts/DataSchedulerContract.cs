using System;

namespace Kepler.Common.CommunicationContracts
{
    public class DataSchedulerContract
    {
        public string SchedulerName { get; set; }
        public TimeSpan SchedulePeriod { get; set; }
        public long HistoryItemsNumberToPreserve { get; set; }
    }
}