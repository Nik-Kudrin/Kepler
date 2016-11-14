using Kepler.Integration.Common.UI.Page.Common;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class PixelItem : CommonComponent
    {
        public string Url { get; set; }
        public string BankName { get; set; }
        public string DepositName { get; set; }

        public override int GetHashCode()
        {
            return (Url + BankName + DepositName).GetHashCode();
        }

        public override bool Equals(object anotherObj)
        {
            var anotherPixel = (PixelItem) anotherObj;

            return (Url == anotherPixel.Url &&
                    BankName == anotherPixel.BankName &&
                    DepositName == anotherPixel.DepositName);
        }


        public override string ToString()
        {
            return string.Format("PixelItem: Url: {0}; BankName: {1}; DepositName: {2}", Url, BankName, DepositName);
        }
    }
}