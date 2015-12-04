﻿using System;
using System.Runtime.CompilerServices;
using System.Timers;
using AutoMapper.Internal;
using Kepler.Common.Repository;

namespace Kepler.Service.Scheduler
{
    public class LogCleanDataScheduler : CleanDataSchedulerBase
    {
        public LogCleanDataScheduler() : base("logCleanScheduler")
        {
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override void CleanData(object sender, ElapsedEventArgs eventArgs)
        {
            base.CleanData(sender, eventArgs);
            var errorRepo = ErrorMessageRepository.Instance;

            var errorsForDelete = errorRepo.Find(
                item => item.Time.Value < DateTime.Now - SchedulerInfo.SchedulePeriod);

            errorsForDelete.Each(errorRepo.Delete);
        }
    }
}