using System;
using System.Collections.Generic;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class BankPropositionComponent<T> : CommonComponent where T : PropositionItem, new()
    {
        public T PropositionItem { get; set; }
        private By _groupSelector = By.CssSelector("ul.results-group-container li.results-container-line");
        private By _expandOrHideGroupsBtn = By.CssSelector("span.results-group-btn");
        private By _hideGroupBtn = By.CssSelector("span.results-group-btn--hide");
        private By _branchesLink = By.CssSelector("div.result-rating  div.company-rating-offices > a");


        public virtual void Init(IWebElement element)
        {
            PropositionItem = GetComponent<T>();
            PropositionItem.Init(element);
        }

        public virtual PropositionItem GetTopItemResult()
        {
            var topElement = PropositionItem.Element.FindElement(By.CssSelector("div.T-DefaultProposition"));
            var topElementComponent = GetComponent<T>();
            topElementComponent.Init(topElement);

            return topElementComponent;
        }

        public virtual IEnumerable<T> GetResultGroup()
        {
            var fakeActionToExpandGroups = false;

            var groupExpandBtn = PropositionItem.Element.WaitAndFindElement(_expandOrHideGroupsBtn, item => item.Displayed,
                Browser, TimeSpan.FromSeconds(2));

            if (groupExpandBtn == null)
                return null;
            else
            {
                Scroll().ScrollToElement(groupExpandBtn);

                var hideGroupBtn = PropositionItem.Element.WaitAndFindElement(_hideGroupBtn, item => item.Displayed,
                    Browser, TimeSpan.FromSeconds(2));

                if (hideGroupBtn == null || !hideGroupBtn.Displayed)
                {
                    groupExpandBtn.Click();
                    fakeActionToExpandGroups = true;
                }
            }

            var groupElements = PropositionItem.Element.FindElements(_groupSelector);

            if (groupElements == null)
                return null;


            var groupItems = new List<T>();

            foreach (var item in groupElements)
            {
                var component = GetComponent<T>();
                component.Init(item);

                groupItems.Add(component);
            }

            if (fakeActionToExpandGroups)
            {
                var hideGroupBtn = PropositionItem.Element.WaitAndFindElement(_hideGroupBtn, item => item.Displayed,
                    Browser, TimeSpan.FromSeconds(2));
                hideGroupBtn.Click();
            }

            return groupItems;
        }

        public virtual void WaitUntilAllGroupElementsInvisible()
        {
            GetDriver().WaitUntilElementInvisible(_groupSelector);
        }

        public virtual string GetUserRate()
        {
            IWebElement userRating = null;
            try
            {
                userRating = PropositionItem.Element.FindElement(By.CssSelector("div.company-rating-reviews span"));
            }
            catch (Exception ex)
            {
                return "0";
            }

            var ratingText = userRating.Text;

            return ratingText == "" ? "0" : ratingText;
        }

        public virtual void ClickExpandOrHideGroups()
        {
            var element = PropositionItem.Element.WaitAndFindElement(_expandOrHideGroupsBtn, item => item.Displayed,
                Browser, TimeSpan.FromSeconds(2));

            if (element != null)
                element.Click();
        }

        public virtual void ClickLinkRedirectToBranches()
        {
            var element = PropositionItem.Element.WaitAndFindElement(_branchesLink, item => item.Displayed,
                Browser, TimeSpan.FromSeconds(2));

            if (element != null)
                element.Click();
        }
    }
}