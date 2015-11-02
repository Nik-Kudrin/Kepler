using System;
using System.IO;
using ImageMagick;
using Kepler.Common.CommunicationContracts;
using Kepler.ImageProcessor.Service.RestKeplerClient;

namespace Kepler.ImageProcessor.Service.ImgProcessor
{
    public class ImageComparator
    {
        public ImageComparisonInfo ImageComparisonInfo { get; set; }

        public ImageComparator()
        {
        }

        public ImageComparator(ImageComparisonInfo imageComparisonInfo)
        {
            this.ImageComparisonInfo = imageComparisonInfo;
        }


        public ImageComparisonInfo GetCompositeImageDiff()
        {
            using (var firstImage = new MagickImage(ImageComparisonInfo.FirstImagePath))
            using (var secondImage = new MagickImage(ImageComparisonInfo.SecondImagePath))
            {
                ImageComparisonInfo.IsImagesDifferent = false;

                var diffFileName = $"{ImageComparisonInfo.ScreenShotName}_{Guid.NewGuid()}";

                ImageComparisonInfo.DiffImagePath = Path.Combine(ImageComparisonInfo.DiffImagePath,
                    diffFileName + ".png");
                ImageComparisonInfo.DiffPreviewPath = Path.Combine(ImageComparisonInfo.DiffPreviewPath,
                    diffFileName + "_preview.png");

                var errorMessage = "";

                //Write preview for first image
                ImageComparisonInfo.FirstPreviewPath = ImageComparisonInfo.FirstImagePath + "_preview.png";
                WritePreviewImage(firstImage, ImageComparisonInfo.FirstPreviewPath);

                if (firstImage.GetHashCode() == secondImage.GetHashCode())
                {
                    // because images are equal, just save first image
                    errorMessage = WriteImageWithPreview(firstImage, ImageComparisonInfo.DiffImagePath,
                        ImageComparisonInfo.DiffPreviewPath);
                    if (errorMessage != "")
                    {
                        ImageComparisonInfo.ErrorMessage = errorMessage;
                    }

                    return ImageComparisonInfo;
                }

                // Generate Diff
                firstImage.Composite(secondImage, CompositeOperator.Difference);

                errorMessage = WriteImageWithPreview(firstImage, ImageComparisonInfo.DiffImagePath,
                    ImageComparisonInfo.DiffPreviewPath);
                if (errorMessage != "")
                {
                    ImageComparisonInfo.ErrorMessage = errorMessage;
                    return ImageComparisonInfo;
                }

                ImageComparisonInfo.IsImagesDifferent = true;
                return ImageComparisonInfo;
            }
        }

        private string WriteImageWithPreview(MagickImage image, string imagePathToSave, string previewPathToSave)
        {
            var errorMessage = WriteImage(image, imagePathToSave);
            errorMessage += WritePreviewImage(image, previewPathToSave);

            return errorMessage;
        }

        private string WriteImage(MagickImage image, string pathToSave)
        {
            try
            {
                image.Write(pathToSave);
            }
            catch (Exception ex)
            {
                var errorMessage =
                    $"Something bad happend in attempt to write file with diff screenshot '{pathToSave}'. {ex.Message}";
                new RestKeplerServiceClient().LogError(errorMessage);

                return errorMessage;
            }
            return "";
        }

        private string WritePreviewImage(MagickImage image, string pathToSave)
        {
            var clonedImage = image.Clone();
            clonedImage.Resize(118, 96);

            var errorMessage = WriteImage(clonedImage, pathToSave);
            if (errorMessage != "")
            {
                return errorMessage;
            }
            return "";
        }

        public ImageComparisonInfo GetCompareImageDiff()
        {
            using (var firstImage = new MagickImage(ImageComparisonInfo.FirstImagePath))
            using (var secondImage = new MagickImage(ImageComparisonInfo.SecondImagePath))
            using (var diffImage = new MagickImage())
            {
                ImageComparisonInfo.IsImagesDifferent = false;

                if (firstImage.GetHashCode() == secondImage.GetHashCode())
                    return ImageComparisonInfo;

                firstImage.Compare(secondImage, ErrorMetric.Absolute, diffImage, Channels.Index);
                diffImage.Write(ImageComparisonInfo.DiffImagePath);
                return ImageComparisonInfo;
            }
        }
    }
}