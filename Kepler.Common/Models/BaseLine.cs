using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kepler.Core
{
    public class BaseLine
    {
        [Key]
        public long Id { get; set; }

        public Dictionary<long?, ScreenShot> ScreenShots { get; set; }
    }
}