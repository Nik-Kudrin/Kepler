using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kepler.Models
{

    public class KeplerDataContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public KeplerDataContext()
            : base("Kepler")
        {
        }
    }
}