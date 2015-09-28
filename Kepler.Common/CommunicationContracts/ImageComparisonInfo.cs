namespace Kepler.Common.CommunicationContracts
{
    public class ImageComparisonInfo
    {
        public long ScreenShotId { get; set; }
        public bool IsImagesDifferent { get; set; }
        public string ErrorMessage { get; set; }

        public string FirstImagePath { get; set; }
        public string SecondImagePath { get; set; }
        public string DiffImgPathToSave { get; set; }

        public override string ToString()
        {
            return
                $"ImageInfo: ScreenShotId: {ScreenShotId}; IsImagesDifferent: {IsImagesDifferent}; ErrorMessage: {ErrorMessage}; FirstImagePath: {FirstImagePath}; SecondImagePath: {SecondImagePath}; DiffImgPathToSave: {DiffImgPathToSave}";
        }
    }
}