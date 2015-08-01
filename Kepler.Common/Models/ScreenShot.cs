using System.ComponentModel.DataAnnotations;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class ScreenShot : BuildObject
    {
        [StringLength(500)]
        public string ImagePath { get; set; }

        public ScreenShot()
        {
        }

        public ScreenShot(string Name, string ImagePath) : base(Name)
        {
            this.ImagePath = ImagePath;
        }
    }
}