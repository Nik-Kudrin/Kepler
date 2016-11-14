using System.Collections.Generic;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class BankProductModel
    {
        public string ProductName { get; set; }
        public IEnumerable<string> ExpectedTextsOnPage { get; set; }
        public IEnumerable<string> ExpectedLinksOnPage { get; set; }
    }
}