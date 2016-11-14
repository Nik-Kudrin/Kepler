using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class RatingComponent : UiComponent
    {
        public IWebElement RatingElement { get; set; }

        public string GetRatingTitle()
        {
            return RatingElement.FindElement(By.CssSelector("div.organization-card__ratings__rating__title")).Text;
        }
    }
}