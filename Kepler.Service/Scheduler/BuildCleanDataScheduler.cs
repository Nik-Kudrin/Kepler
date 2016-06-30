using System;
using System.Collections.Generic;
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
                BuildRepository.Instance.Find(string.Format("WHERE StartDate IS NOT NULL AND StartDate < '{0}'",
                    (DateTime.Now - SchedulerInfo.SchedulePeriod).ToString("yyyy-MM-dd HH:mm:ss")))
                    .GroupBy(item => item.BranchId);

            foreach (var builds in buildInBranches)
            {
                var buildsForRemoving = new List<Build>();

                foreach (var build in builds)
                {
                    // if build has any IsLastPassed screenshot - we cannot delete this build, because it contains in BaseLine
                    if (!ScreenShotRepository.Instance.FindByBuildId(build.Id)
                        .Any(screenshot => screenshot.IsLastPassed))
                    {
                        buildsForRemoving.Add(build);
                    }
                }

                buildsForRemoving.Skip(SchedulerInfo.HistoryItemsNumberToPreserve)
                    .Each(item => DataCleaner.DeleteObjectsTreeRecursively<Build>(item.Id, true));
            }
        }
    }
}