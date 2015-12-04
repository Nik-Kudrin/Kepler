using System;

namespace Kepler.Common.CommunicationContracts
{
    public class DataSchedulerContract
    {
        public string Name { get; set; }
        public TimeSpan SchedulePeriod { get; set; }
        public DateTime? LastStartTime { get; set; }
        public DateTime? NextStartTime { get; set; }
        public int HistoryItemsNumberToPreserve { get; set; }
    }
}