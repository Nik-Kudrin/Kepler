using System;
using System.Collections.Generic;
using Kepler.Core;

namespace Kepler.Service
{
    public class KeplerService : IKeplerService
    {
        private BuildRepository buildRepo = BuildRepository.Instance;

        public Build GetBuild(string id)
        {
            return buildRepo.Get(Convert.ToInt64(id));
        }

        public IEnumerable<Build> GetBuilds()
        {
            return buildRepo.FindAll();
        }
    }
}