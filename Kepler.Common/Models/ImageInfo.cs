namespace Kepler.Common.Models
{
    public class ImageInfo
    {
        public long ScreenShotId { get; set; }
        public bool IsImagesDifferent { get; set; }
        public string ErrorMessage { get; set; }

        public string FirstImagePath { get; set; }
        public string SecondImagePath { get; set; }
        public string DiffImgPathToSave { get; set; }
    }
}