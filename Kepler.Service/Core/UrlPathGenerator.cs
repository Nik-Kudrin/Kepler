using Kepler.Common.Models;

namespace Kepler.Service.Core
{
    public class UrlPathGenerator
    {
        public static string DiffImagePath = new KeplerService().GetDiffImageSavingPath();
        public static string SourceImagePath = new KeplerService().GetSourceImagePath();

        private static string GenerateUrlPath(string filePath, string replacePath, string urlBasePath)
        {
            if (!string.IsNullOrEmpty(filePath))
                return filePath.ToLowerInvariant().Replace(replacePath, urlBasePath).Replace(@"\", "/");

            return "";
        }

        private static string GenerateDiffUrlPath(string filePath)
        {
            return GenerateUrlPath(filePath, DiffImagePath, "img/diff");
        }


        private static string GenerateOriginUrlPath(string filePath)
        {
            return GenerateUrlPath(filePath, SourceImagePath, "img/origin");
        }

        public static void ReplaceFilePathWithUrl(ScreenShot screenShot)
        {
            screenShot.ImagePathUrl = GenerateOriginUrlPath(screenShot.ImagePath);
            screenShot.PreviewImagePathUrl = GenerateOriginUrlPath(screenShot.PreviewImagePath);

            screenShot.BaseLineImagePathUrl = GenerateOriginUrlPath(screenShot.BaseLineImagePath);
            screenShot.BaseLinePreviewPathUrl = GenerateOriginUrlPath(screenShot.BaseLinePreviewPath);

            screenShot.DiffImagePathUrl = GenerateDiffUrlPath(screenShot.DiffImagePath);
            screenShot.DiffPreviewPathUrl = GenerateDiffUrlPath(screenShot.DiffPreviewPath);
        }
    }
}