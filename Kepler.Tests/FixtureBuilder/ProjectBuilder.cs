using System.Collections.Generic;
using Kepler.Core;
using Kepler.Models;

namespace Kepler.Tests.FixtureBuilder
{
    public class ProjectBuilder : Project, IFixtureBuilder<Project>
    {
        public Project BuildValid()
        {
            BaseLine = new BaseLine();
            Builds = new Dictionary<long?, Build>();

            return this;
        }
    }
}