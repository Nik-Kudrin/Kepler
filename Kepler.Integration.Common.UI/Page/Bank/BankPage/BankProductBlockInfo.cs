using System;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.BankPage
{
    public class BankProductBlockInfo : CommonComponent
    {
        public BankListingPage<PropositionItem>.BankProductType ProductType { get; set; }
        public string Title { get; set; }
        public string RelativeUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsIconDisplayed { get; set; }

        public IWebElement Element { get; set; }

        public BankProductBlockInfo(IWebElement element)
        {
            Element = element;
        }

        public BankProductBlockInfo(string productType)
            : this(ConvertProductNameToType(productType))
        {
        }

        public BankProductBlockInfo(BankListingPage<PropositionItem>.BankProductType productType)
        {
            switch (productType)
            {
                case BankListingPage<PropositionItem>.BankProductType.Credit:
                    Title = "Взять кредит";
                    RelativeUrl = "kredity/";
                    break;
                case BankListingPage<PropositionItem>.BankProductType.Mortgage:
                    Title = "Выбрать ипотеку";
                    RelativeUrl = "ipoteka/";
                    break;
                case BankListingPage<PropositionItem>.BankProductType.Deposit:
                    Title = "Сделать вклад";
                    RelativeUrl = "vklady/";
                    break;
                case BankListingPage<PropositionItem>.BankProductType.CreditCard:
                    Title = "Подобрать кредитную карту";
                    RelativeUrl = "karty/";
                    break;
                case BankListingPage<PropositionItem>.BankProductType.DebitCard:
                    Title = "Оформить дебетовую карту";
                    RelativeUrl = "debetovye-karty/";
                    break;
                case BankListingPage<PropositionItem>.BankProductType.AutoCredit:
                    Title = "Выбрать автокредит";
                    RelativeUrl = "avtokredity/";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static BankListingPage<PropositionItem>.BankProductType ConvertProductNameToType(string productType)
        {
            switch (productType)
            {
                case "kredity":
                    return BankListingPage<PropositionItem>.BankProductType.Credit;
                case "ipoteka":
                    return BankListingPage<PropositionItem>.BankProductType.Mortgage;
                case "vklady":
                    return BankListingPage<PropositionItem>.BankProductType.Deposit;
                case "karty":
                    return BankListingPage<PropositionItem>.BankProductType.CreditCard;
                case "debetovye-karty":
                    return BankListingPage<PropositionItem>.BankProductType.DebitCard;
                case "avtokredity":
                    return BankListingPage<PropositionItem>.BankProductType.AutoCredit;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}