using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Kepler.Integration.Common.UI.SelenoExtension
{
    public class SliderComponent
    {
        private readonly IWebDriver _driver;


        public SliderComponent(IWebDriver driver)
        {
            _driver = driver;
        }


        public double GetSliderPositionInPercents(By selector)
        {
            var element = _driver.FindElement(selector);
            return GetSliderPositionInPercents(element);
        }

        public double GetSliderPositionInPercents(IWebElement element)
        {
            var firstValueSliderStr =
                element.FindElement(By.CssSelector("div > div.ui-slider-range"))
                    .GetAttribute("style")
                    .Replace("width: ", "")
                    .Replace("%;", "")
                    .Replace(".", ",");

            return double.Parse(firstValueSliderStr);
        }


        public void MoveSlider(By selector, double valueInPercents)
        {
            var element = _driver.FindElement(selector);
            MoveSlider(element, valueInPercents);
        }

        public void MoveSlider(IWebElement element, double valueInPercents)
        {
            var sliderWidth = element.Size.Width;
            var onePersencInPixels = (double) sliderWidth/100;
            var moveInPixels = onePersencInPixels*valueInPercents - (double) sliderWidth/2;

            var actions = new Actions(_driver);
            //Есть особенность сначала бегунок двигается в "0" положение т.е. ровно в середину слайдера, а потом уже по указанным координатам
            actions.DragAndDropToOffset(element, (int) moveInPixels, 0);
            actions.Perform();
            Thread.Sleep(1000);
        }
    }
}