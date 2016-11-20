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
                var previewSuffix = "_preview.png";
                var randomId = Guid.NewGuid().ToString().Substring(0, 8);
                var diffFileName = $"{ImageComparisonInfo.ScreenShotName}_{randomId}";

                // in case - rerun build just use old generated paths
                if (!ImageComparisonInfo.DiffImagePath.Contains(ImageComparisonInfo.ScreenShotName))
                {
                    ImageComparisonInfo.DiffImagePath = Path.Combine(ImageComparisonInfo.DiffImagePath,
                        diffFileName + ".png");
                    ImageComparisonInfo.DiffPreviewPath = Path.Combine(ImageComparisonInfo.DiffPreviewPath,
                        diffFileName + previewSuffix);
                }
                var errorMessage = "";

                //Write preview for first image (old screenshot - baseline)

                // in case - rerun build just use old generated paths
                if (ImageComparisonInfo.FirstPreviewPath == null ||
                    !ImageComparisonInfo.FirstPreviewPath.EndsWith(previewSuffix))
                {
                    ImageComparisonInfo.FirstPreviewPath = ImageComparisonInfo.FirstImagePath + "_preview.png";
                }

                ImageComparisonInfo.SecondPreviewPath = ImageComparisonInfo.SecondImagePath + "_preview.png";
                WritePreviewImage(firstImage, ImageComparisonInfo.FirstPreviewPath);
                WritePreviewImage(secondImage, ImageComparisonInfo.SecondPreviewPath);

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
                var errorMessage = $"Attempt to write file failed. '{pathToSave}'. {ex.Message} {ex.StackTrace}";
                new RestKeplerServiceClient().LogError(errorMessage);

                return errorMessage;
            }
            return "";
        }

        private string WritePreviewImage(MagickImage image, string pathToSave)
        {
            using (var clonedImage = image.Clone())
            {
                clonedImage.Resize(new ImageMagick.MagickGeometry("200x163!"));

                var errorMessage = WriteImage(clonedImage, pathToSave);
                if (errorMessage != "")
                {
                    return errorMessage;
                }
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