using ImageMagick;
using Kepler.Common.Models;

namespace KeplerImageProcessorService.ImgProcessor
{
    public class ImageProcessor
    {
        public ImageInfo ImageInfo { get; set; }

        public ImageProcessor()
        {
        }

        public ImageProcessor(ImageInfo imageInfo)
        {
            this.ImageInfo = imageInfo;
        }


        public ImageInfo GetCompositeImageDiff()
        {
            using (var firstImage = new MagickImage(ImageInfo.FirstImagePath))
            using (var secondImage = new MagickImage(ImageInfo.SecondImagePath))
            {
                ImageInfo.IsImagesDifferent = false;

                if (firstImage.GetHashCode() == secondImage.GetHashCode())
                    return ImageInfo;

                firstImage.Composite(secondImage, CompositeOperator.Difference);
                firstImage.Write(ImageInfo.DiffImgPathToSave);
                ImageInfo.IsImagesDifferent = true;
                return ImageInfo;
            }
        }

        public ImageInfo GetCompareImageDiff()
        {
            using (var firstImage = new MagickImage(ImageInfo.FirstImagePath))
            using (var secondImage = new MagickImage(ImageInfo.SecondImagePath))
            using (var diffImage = new MagickImage())
            {
                ImageInfo.IsImagesDifferent = false;

                if (firstImage.GetHashCode() == secondImage.GetHashCode())
                    return ImageInfo;

                firstImage.Compare(secondImage, ErrorMetric.Absolute, diffImage, Channels.Index);
                diffImage.Write(ImageInfo.DiffImgPathToSave);
                return ImageInfo;
            }
        }
    }
}