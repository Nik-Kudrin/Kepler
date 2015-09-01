using System.Collections.Generic;
using Kepler.Core;
using Kepler.Models;

namespace Kepler.Tests.FixtureBuilder
{
    public class ProjectBuilder : Project, IFixtureBuilder<Project>
    {
        public Project Build()
        {
            BaseLine = new BaseLine();
            Builds = new Dictionary<long?, Build>();

            return this;
        }

        public Project BuildValid()
        {
            

            return this;
        }
    }
}