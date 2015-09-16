using System.Collections.Generic;
using KeplerImageProcessorService.ImageProcessor;
using NUnit.Framework;

namespace Kepler.Tests.Test
{
    public class ImageTest : InitTest
    {
/*
    @"e:\Temp\ScreenShot_Samples\19212-nebula-1920x1080-space-wallpaper.jpg"
    @"e:\Temp\ScreenShot_Samples\ElementFinder_2015-07-29_15-51-00.png"
    @"e:\Temp\ScreenShot_Samples\19212-nebula-1920x1080-space-wallpaper.jpg"
    @"e:\Temp\ScreenShot_Samples\20121011_mars_msl_sol50_pano_east-hills_0050MR0229039000E1_DXXX.jpg"
    @"e:\Temp\ScreenShot_Samples\20130121_jupiter_vgr1_c1620_sat_transits.jpg"
    @"e:\Temp\ScreenShot_Samples\ElementFinder_2015-07-29_15-51-21.png"
*/

        [Test]
        public void DiffImageTest()
        {
            var images = new[]
            {
                @"e:\Temp\ScreenShot_Samples\ElementFinder_2015-07-29_15-51-00.png",
                @"e:\Temp\ScreenShot_Samples\ElementFinder_2015-07-29_15-51-21.png",
              /*  "https://lh3.googleusercontent.com/6boDIiWTlifjqcu0_J_8L65RUqH6SbvDVDO_k4g4R9pDtbJi9uBVJBC6ku3wrd39ACNci2W0IJoLLUY1Rmhj1qgnkGxVMLWUIneNb66Gow=s660",
                "http://lh3.googleusercontent.com/wshxz9Sz_qh1CqiyBmlOp2eWgyXFWTjRoR4HEmG3onEmq4Vh-vlGnYnGcE_P3GQFHqfYzLqLRP3PWBMOS7A84d6L85ZQXku7DJoFO-I=s660",
                "https://www.google.com/doodle4google/2014/images/doodles/4-10.jpg",
                "http://www.da-files.com/artnetwork/zeitgeist/google-doodle/57-img-31.jpg",
                "http://static.ibnlive.in.com/pix/slideshow/08-2013/googles-independence-day/08-google-india-independence-day-doodle-150812.jpg",
                "https://cdn3.vox-cdn.com/thumbor/JVYydIGH4ZyY4sb2rzXcxqKP6UI=/cdn0.vox-cdn.com/uploads/chorus_asset/file/3870858/google-doodle.0.gif"*/
            };
            var outputFile = @"e:\Temp\ScreenShot_Samples\result";
            var imageProcessor = new ImageProcessor();

            for (int index = 0; index < images.Length;)
            {
                imageProcessor.GetImageDiffWithCompare(images[index], images[index + 1], outputFile + "_" + index + ".png");
                index += 2;
            }
        }
    }
}