using System;
using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.Page.Kasko.PopUps;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko
{
    public class KaskoPage : CommonPage
    {
        public readonly By LoadProcessSelector = By.CssSelector("div.serp-loader-container > div.serp-loader");

        public List<ResultCard> GetResultsList()
        {
            WaitUntilElementVisible(LoadProcessSelector, TimeSpan.FromSeconds(60));

            var resultsWebElems =
                Find.Element(By.CssSelector("div.serp-results"))
                    .FindElements(By.CssSelector("ul.results-container > li.result-card-big"));

            var resultsCards = new List<ResultCard>();
            foreach (var resultsWebElem in resultsWebElems)
            {
                if (resultsWebElem.Displayed)
                {
                    resultsCards.Add(new ResultCard(resultsWebElem));
                }
            }

            return resultsCards;
        }

        public BrandsListComponent GetPageBrandsListComponent()
        {
            return GetComponent<BrandsListComponent>();
        }

        public RadioControlComponent GetPageWithRadioControlComponent()
        {
            return GetComponent<RadioControlComponent>();
        }

        public CarPriceAndMileageComponent GetPageCarPriceComponent()
        {
            return GetComponent<CarPriceAndMileageComponent>();
        }

        public DriverInfoPage GetDriverInfoPage()
        {
            const string selector = "div.serp-calculator-inner-container";

            return new DriverInfoPage(Find.Element(By.CssSelector(selector)));
        }

        public ConfirmPopupComponent GetConfirmPopupPage()
        {
            return GetComponent<ConfirmPopupComponent>();
        }

        public ConfirmEmailPopupComponent GetConfirmEmailPopupComponent()
        {
            return GetComponent<ConfirmEmailPopupComponent>();
        }

        public OrderInfoPopupComponent GetOrderInfoPopupComponent()
        {
            return GetComponent<OrderInfoPopupComponent>();
        }

        public KaskoFilterComponent GetKaskoFilterComponent()
        {
            return GetComponent<KaskoFilterComponent>();
        }


        public int GetResultsCount()
        {
            //TODO так как дальше придется работать с результатами этот метод нужно будет изменить
            var listsResult =
                Find.Element(By.CssSelector("div.serp-results"))
                    .FindElements(By.CssSelector("ul.results-container > li.result-card-big"));

            return listsResult.Count;
        }


        public void ClickDropDownOpenInSettings(string titleDropDown)
        {
            var mainSelector = "div.calculator-controls-container.l-row.add-margin";
            var elemSelector = "div.l-col > div";

            SeceltFromList(mainSelector, elemSelector, titleDropDown);
        }

        public void ClickByElementFromDropDownInSettings(string elem)
        {
            var mainSelector =
                "body > div > div.ik_select_list > div.ik_select_list_inner > ul";
            var elemSelector = " li > span";

            SeceltFromList(mainSelector, elemSelector, elem);
        }


        public void ClickDropDownOpenInResults(string titleDropDown)
        {
            var mainSelector = "form > div.calculator-controls-container.l-row";
            var elemSelector = "div.l-col > div";

            SeceltFromList(mainSelector, elemSelector, titleDropDown);
        }

        public void ClickByElementFromDropDownInResults(string elem)
        {
            var mainSelector =
                "body > div.ik_select_dropdown.select-control-inner--medium-dd.transition.animate > div > div > ul";
            var elemSelector = " li > span";

            SeceltFromList(mainSelector, elemSelector, elem);
        }


        public void ClickDropDownOpenSortResults()
        {
            var mainSelector =
                "div.serp-results > form > div > div > div.ik_select_link.select-control-inner--small-link";
            var webElementMain = Find.Element(By.CssSelector(mainSelector));
            webElementMain.Click();
        }

        public void ClickByElementFromDropDownSortResults(string elem)
        {
            var mainSelector =
                "body > div.ik_select_dropdown.select-control-inner--small-dd.transition.animate > div > div > ul";
            var elemSelector = " li > span";

            SeceltFromList(mainSelector, elemSelector, elem);
        }


        public void ClickOnRadioControlElement(string elem)
        {
            var mainSelector = ".radio-control-inner";
            var elemSelector = "ul > li > label";

            SeceltFromList(mainSelector, elemSelector, elem, false);
        }

        public void ClickNoOnRadioControl()
        {
            ClickOnRadioControlElement("Нет");
        }

        public void ClickYesOnRadioControl()
        {
            ClickOnRadioControlElement("Да");
        }

        public void ClickThereIsOnRadioControl()
        {
            ClickOnRadioControlElement("Есть");
        }

        private void SeceltFromList(string mainSelector, string elemSelector, string elem, bool withScroll = true)
        {
            WaitUntilPageIsLoaded();

            var webElementMain = Find.Element(By.CssSelector(mainSelector));
            var webElementsFromList = webElementMain.FindElements(By.CssSelector(elemSelector));
            var webElement = webElementsFromList.FirstOrDefault(element => element.Text.Contains(elem));

            if (withScroll)
            {
                Scroll().ScrollToElement(webElement);
            }

            if (webElement != null) webElement.Click();
        }

        public void EnterStringInSliderByFieldLabel(string stringValue, string fieldLabel)
        {
            var sliderFields =
                Find.Elements(
                    By.CssSelector("form > div.calculator-controls-container.l-row > div.l-col > div.slider-control"));

            var slider = sliderFields.FirstOrDefault(element => element.Text.Contains(fieldLabel));
            var sliderInput = slider.FindElement(By.CssSelector("input"));
            sliderInput.Clear();
            sliderInput.SendKeys(stringValue);
        }

    }
}