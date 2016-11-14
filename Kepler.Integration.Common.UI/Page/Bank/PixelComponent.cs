using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank
{
    public class PixelComponent<T> : CommonComponent where T : PropositionItem, new()
    {
        public IEnumerable<PixelItem> GetVisiblePixels()
        {
            var pixels = new List<PixelItem>();

            var propositions = GetComponent<SpecialPropositionListingComponent<T>>().GetPropositionsResult().ToList();
            propositions.AddRange(GetSuperSpecialPropositions());
            propositions.AddRange(GetComponent<StandardPropositionListingComponent<T>>().GetPropositionsResult());

            foreach (var proposition in propositions)
            {
                var itemProposition = proposition.PropositionItem;

                var pixelUrl = itemProposition.GetPixelUrl();
                // skip items, where pixels doesn't exist
                if (pixelUrl == null)
                    continue;

                var pixel = new PixelItem()
                {
                    Url = pixelUrl,
                    BankName = itemProposition.GetProductBankName(),
                    DepositName = itemProposition.GetProductName()
                };

                pixels.Add(pixel);
            }

            return pixels;
        }

        public virtual IEnumerable<BankPropositionComponent<T>> GetSuperSpecialPropositions()
        {
            return GetComponent<SpecialPropositionListingComponent<T>>().GetSuperSpecialPropositions();
        }

        public IEnumerable<PixelItem> GetVisiblePixelsForGroup(BankPropositionComponent<T> propositionWithPixels)
        {
            var pixels = new List<PixelItem>();

            var bankName = propositionWithPixels.PropositionItem.GetProductBankName();

            var group = propositionWithPixels.GetResultGroup();

            foreach (var itemProposition in group)
            {
                var pixelUrl = itemProposition.GetPixelUrl();
                // skip items, where pixels doesn't exist
                if (pixelUrl == null)
                    continue;

                var pixel = new PixelItem()
                {
                    Url = pixelUrl,
                    BankName = bankName,
                    DepositName = itemProposition.GetProductName()
                };

                pixels.Add(pixel);
            }

            return pixels;
        }


        public PixelItem GetBinbankSpecialPixelUrl()
        {
            IWebElement binBankLogo;
            try
            {
                binBankLogo = Find.Element(By.CssSelector("a.branding-logo-container"), TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                return null;
            }

            var binBankHref = binBankLogo.GetAttribute("href");
            binBankHref = HttpUtility.UrlDecode(binBankHref.Substring(binBankHref.IndexOf("=http") + 1));

            return new PixelItem()
            {
                Url = binBankHref,
                BankName = "БинБанк",
                DepositName = "Это ссылка - изображение спонсора раздела"
            };
        }
    }
}