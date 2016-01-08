namespace Kepler.Common.CommunicationContracts
{
    public class ImageComparisonInfo
    {
        public string ScreenShotName { get; set; }

        public long ScreenShotId { get; set; }
        public long? LastPassedScreenShotId { get; set; }
        public bool IsImagesDifferent { get; set; }
        public string ErrorMessage { get; set; }

        public string FirstImagePath { get; set; }
        public string FirstPreviewPath { get; set; }

        public string SecondImagePath { get; set; }
        public string SecondPreviewPath { get; set; }

        public string DiffImagePath { get; set; }
        public string DiffPreviewPath { get; set; }

        public ImageComparisonInfo()
        {
            ErrorMessage = string.Empty;
        }

        public override string ToString()
        {
            return
                $"ImageInfo: ScreenShotId: {ScreenShotId}; IsImagesDifferent: {IsImagesDifferent}; ErrorMessage: {ErrorMessage}; FirstImagePath: {FirstImagePath}; SecondImagePath: {SecondImagePath}; DiffImgPathToSave: {DiffImagePath}";
        }
    }
}