using ImageMagick;
using Kepler.Common.Models;

namespace KeplerImageProcessorService.ImgProcessor
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

                if (firstImage.GetHashCode() == secondImage.GetHashCode())
                    return ImageComparisonInfo;

                firstImage.Composite(secondImage, CompositeOperator.Difference);
                firstImage.Write(ImageComparisonInfo.DiffImgPathToSave);
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