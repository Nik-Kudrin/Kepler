﻿using System.Collections.Generic;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BuildRepository : BaseObjRepository<Build>
    {
        public static BuildRepository Instance => new BuildRepository();

        private BuildRepository()
        {
        }

        public IEnumerable<Build> GetBuildsByStatus(ObjectStatus status)
        {
            return Find(new {Status = status});
        }
    }
}