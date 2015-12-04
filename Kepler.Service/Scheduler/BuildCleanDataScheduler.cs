using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using AutoMapper.Internal;
using Kepler.Common.Models;
using Kepler.Common.Repository;
using Kepler.Service.Core;

namespace Kepler.Service.Scheduler
{
    public class BuildCleanDataScheduler : CleanDataSchedulerBase
    {
        public BuildCleanDataScheduler() : base("buildCleanScheduler")
        {
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override void CleanData(object sender, ElapsedEventArgs eventArgs)
        {
            base.CleanData(sender, eventArgs);

            var buildInBranches =
                BuildRepository.Instance.Find(item => item.StartDate.HasValue &&
                                                      item.StartDate < DateTime.Now - SchedulerInfo.SchedulePeriod)
                    .GroupBy(item => item.BranchId);

            foreach (var builds in buildInBranches)
            {
                builds.Skip(SchedulerInfo.HistoryItemsNumberToPreserve)
                    .Each(item => DataCleaner.DeleteObjectsTreeRecursively<Build>(item.Id, true));
            }
        }
    }
}