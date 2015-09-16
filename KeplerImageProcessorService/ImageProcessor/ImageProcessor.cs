using System;
using System.Drawing;
using ImageMagick;

namespace KeplerImageProcessorService.ImageProcessor
{
    public class ImageProcessor
    {
        public bool GetImageDiff(string firstImagePath, string secondImagePath, string diffImgPathToSave)
        {
            using (var firstImage = new MagickImage(firstImagePath))
            using (var secondImage = new MagickImage(secondImagePath))
            {
                if (firstImage.GetHashCode() == secondImage.GetHashCode())
                    return true;

                firstImage.Composite(secondImage, CompositeOperator.Difference);
                firstImage.Write(diffImgPathToSave);
                return false;
            }
        }

        public bool GetImageDiffWithCompare(string firstImagePath, string secondImagePath, string diffImgPathToSave)
        {
            using (var firstImage = new MagickImage(firstImagePath))
            using (var secondImage = new MagickImage(secondImagePath))
            using (var diffImage = new MagickImage())
            {
                if (firstImage.GetHashCode() == secondImage.GetHashCode())
                    return true;

                firstImage.Compare(secondImage, ErrorMetric.Absolute, diffImage, Channels.Index);
                diffImage.Write(diffImgPathToSave);
                return false;
            }
        }
    }
}