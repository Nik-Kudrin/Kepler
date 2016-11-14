using System;
using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.BankPage
{
    public class BankProductsBlockComponent : CommonPage
    {
        private string _bankProductBlockSelector = "div.company-page-main ";

        /// <summary>
        /// Return product blocks "Взять кредит", "Оформить ипотеку" ...
        /// </summary>
        /// <param name="getOnlyActiveBlocks">True - return only active blocks. False - return active + not active blocks</param>
        /// <returns></returns>
        public IEnumerable<BankProductBlockInfo> GetBlocks(bool getOnlyActiveBlocks = true)
        {
            var blockWebElements = new List<IWebElement>();

            try
            {
                // In case if some blocks are active
                blockWebElements.AddRange(Find.Elements(By.CssSelector(_bankProductBlockSelector + "a.service-card"), TimeSpan.FromSeconds(5)));
            }
            catch (Exception)
            {
                // ignored
            }
            if (!getOnlyActiveBlocks)
            {
                try
                {
                    // In case if some blocks are INactive
                    blockWebElements.AddRange(Find.Elements(By.CssSelector(_bankProductBlockSelector + "div.service-card"), TimeSpan.FromSeconds(5)));
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            var blocks = new List<BankProductBlockInfo>();
            foreach (var webElement in blockWebElements)
            {
                blocks.Add(InitProductBlockInfo(webElement));
            }

            return blocks;
        }

        public BankListingPage<PropositionItem> ClickBankProductBlock(
            BankListingPage<PropositionItem>.BankProductType productType)
        {
            var productBlocks = GetBlocks();
            var block = productBlocks.FirstOrDefault(_ => _.Title == new BankProductBlockInfo(productType).Title);
            block.Element.Click();

            return GetComponent<BankListingPage<PropositionItem>>();
        }

        private static BankProductBlockInfo InitProductBlockInfo(IWebElement element)
        {
            var blockInfo = new BankProductBlockInfo(element);
            blockInfo.Title = element.FindElement(By.CssSelector("h2")).Text;

            var isActive = !element.GetAttribute("class").Contains("is-disabled");
            blockInfo.IsActive = isActive;
            blockInfo.IsIconDisplayed = element.FindElement(By.CssSelector("span.icon")).Displayed;

            if (isActive)
            {
                var url = element.GetAttribute("href");
                blockInfo.RelativeUrl = url;
                blockInfo.ProductType =
                    BankProductBlockInfo.ConvertProductNameToType(url.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries).Last());
            }

            return blockInfo;
        }
    }
}