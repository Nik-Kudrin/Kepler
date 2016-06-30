using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using Kepler.Common.Error;
using Kepler.Common.Models;
using Kepler.Common.Repository;
using Kepler.Service.RestWorkerClient;

namespace Kepler.Service.Scheduler
{
    public class CheckWorkersScheduler : Scheduler
    {
        private static Scheduler _instance;

        public static Scheduler GetScheduler
        {
            get
            {
                _instance = _instance ?? new CheckWorkersScheduler();
                return _instance;
            }
        }

        protected CheckWorkersScheduler()
        {
        }

        public override void Invoke()
        {
            CheckWorkersAvailability(null, null);
        }

        public override void Enable()
        {
            ScheduleTimer = new Timer {Interval = 60000};
            ScheduleTimer.Elapsed += CheckWorkersAvailability;
            ScheduleTimer.Enabled = true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CheckWorkersAvailability(object sender, ElapsedEventArgs eventArgs)
        {
            var workers = ImageWorkerRepository.Instance.FindAll().ToList();

            if (workers == null || !workers.Any())
                return;

            foreach (var imageWorker in workers)
            {
                try
                {
                    var restImageProcessorClient = new RestImageProcessorClient(imageWorker.WorkerServiceUrl);
                    restImageProcessorClient.SetKeplerServiceUrl();

                    imageWorker.WorkerStatus = ImageWorker.StatusOfWorker.Available;
                }
                catch (Exception ex)
                {
                    ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                    {
                        ExceptionMessage = $"Image worker: '{imageWorker.Name}' is unavailable. {ex.Message}"
                    });
                    imageWorker.WorkerStatus = ImageWorker.StatusOfWorker.Offline;
                }
                finally
                {
                    ImageWorkerRepository.Instance.Update(imageWorker);
                }
            }
        }
    }
}