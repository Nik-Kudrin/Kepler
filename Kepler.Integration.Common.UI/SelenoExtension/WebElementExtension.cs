using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.SelenoExtension
{
    public static class WebElementExtension
    {
        public static void TypeText(this IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);

            // TODO: Maybe we should focus first on element
            // Option 1.
            // element.SendKeys(string.Empty);
            // Option 2.
            // ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", element);
        }
    }
}