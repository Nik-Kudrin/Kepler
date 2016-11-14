using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Organization
{
    public class OrganizationMenuComponent : CommonPage
    {
        protected string BaseMenuSelector = "div.header > div.bottom-container ul > li";

        public IEnumerable<IWebElement> GetItems()
        {
            return Find.Elements(By.CssSelector(BaseMenuSelector)).ToList();
        }

        public string GetOrganizationNameInMenu()
        {
            return GetItems().FirstOrDefault().Text;
        }

        /// <summary>
        /// Саб меню "Продукты банка"
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public T ClickSubMenuOrganizationProduct<T>(string productName) where T : OrganizationPage, new()
        {
            var productsMenu = GetItems().ToList()[1];
            productsMenu.Click();

            var subMenuItem = productsMenu.FindElements(By.CssSelector("ul.sub-list li > a"))
                .FirstOrDefault(_ => _.GetAttribute("href").Contains(productName));
            subMenuItem.Click();

            return GetPage<T>();
        }

        /// <summary>
        /// Меню организации "Отзывы", "Новости" ...
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public T ClickMenuItem<T>(string itemName) where T : OrganizationPage, new()
        {
            var menuSelector = BaseMenuSelector + " > a";
            var items = Find.Elements(By.CssSelector(menuSelector));

            foreach (var menuItem in items)
            {
                try
                {
                    if (menuItem.GetAttribute("href").Contains(itemName))
                    {
                        menuItem.Click();
                        break;
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return GetPage<T>();
        }

        /// <summary>
        /// Существует ли элемент меню организации
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public bool IsMenuItemExist(string itemName)
        {
            var menuSelector = BaseMenuSelector + " > a";
            var items = Find.Elements(By.CssSelector(menuSelector));

            foreach (var menuItem in items)
            {
                try
                {
                    if (menuItem.GetAttribute("href").Contains(itemName))
                    {
                        return true;
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return false;
        }
    }
}