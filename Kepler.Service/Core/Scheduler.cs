using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using Kepler.Common.Error;
using Kepler.Common.Models;
using Kepler.Common.Repository;
using Kepler.Service.RestWorkerClient;

namespace Kepler.Service.Core
{
    public class Scheduler
    {
        private static Timer _updateObjectStatusesTimer;
        private static Timer _buildDataCleanerTimer;
        private static Timer _logDataCleanerTimer;
        private static Timer _checkWorkersTimer;
        private static Scheduler _instance;

        public static Scheduler GetScheduler
        {
            get
            {
                _instance = _instance ?? new Scheduler();
                return _instance;
            }
        }

        private Scheduler()
        {
            _updateObjectStatusesTimer = new Timer {Interval = 15000};
            _updateObjectStatusesTimer.Elapsed += UpdateObjectsStatuses;
            _updateObjectStatusesTimer.Enabled = true;

            _checkWorkersTimer = new Timer {Interval = 60000};
            _checkWorkersTimer.Elapsed += CheckWorkersAvailability;
            _checkWorkersTimer.Enabled = true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateObjectsStatuses(object sender, ElapsedEventArgs eventArgs)
        {
            ObjectStatusUpdater.UpdateAllObjectStatusesToActual();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CheckWorkersAvailability(object sender, ElapsedEventArgs eventArgs)
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
                    ImageWorkerRepository.Instance.UpdateAndFlashChanges(imageWorker);
                }
            }
        }
    }
}