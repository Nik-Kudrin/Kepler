using System.Data.Entity;
using Kepler.Core;

namespace Kepler.Models
{
    public class KeplerDataContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Build> Builds { get; set; }

        public DbSet<ScreenShot> ScreenShots { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<TestSuite> TestSuites { get; set; }
        public DbSet<TestAssembly> TestAssemblies { get; set; }


        public KeplerDataContext()
            : base("Kepler")
        {
        }
    }
}