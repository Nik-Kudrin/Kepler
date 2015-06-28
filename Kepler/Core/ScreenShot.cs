using Kepler.Core.Common;

namespace Kepler.Core
{
    public class ScreenShot : BuildObject
    {
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