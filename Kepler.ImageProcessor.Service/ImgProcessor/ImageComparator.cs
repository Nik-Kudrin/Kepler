using System;
using System.IO;
using ImageMagick;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Error;
using Kepler.Common.Models;

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

                if (!Directory.Exists(ImageComparisonInfo.DiffImgPathToSave))
                {
                    ImageComparisonInfo.ErrorMessage =
                        $"Path for saving screenshots '{ImageComparisonInfo.DiffImgPathToSave}' doesn't exist";
                    return ImageComparisonInfo;
                }

                ImageComparisonInfo.DiffImgPathToSave = Path.Combine(ImageComparisonInfo.DiffImgPathToSave,
                    Guid.NewGuid().ToString() + ".png");

                if (firstImage.GetHashCode() == secondImage.GetHashCode())
                {
                    // because images are equal, just save first image
                    firstImage.Write(ImageComparisonInfo.DiffImgPathToSave);
                    return ImageComparisonInfo;
                }

                firstImage.Composite(secondImage, CompositeOperator.Difference);
                try
                {
                    firstImage.Write(ImageComparisonInfo.DiffImgPathToSave);
                }
                catch (Exception ex)
                {
                    ImageComparisonInfo.ErrorMessage =
                        $"Something bad happend in attempt to write file with diff screenshot '{ImageComparisonInfo.DiffImgPathToSave}'. {ex.Message}";
                    return ImageComparisonInfo;
                }

                ImageComparisonInfo.IsImagesDifferent = true;
                return ImageComparisonInfo;
            }
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
                diffImage.Write(ImageComparisonInfo.DiffImgPathToSave);
                return ImageComparisonInfo;
            }
        }
    }
}