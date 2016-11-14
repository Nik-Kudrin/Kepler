using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Organization
{
    public class OrganizationRatingComponent : CommonPage
    {
        private string clientRatingBlock = "div.clients-rating-block div.l-col";
        private string companyRatingChartBlock = "div.company-rating-charts-block div.l-col";

        public IEnumerable<ClientRatingBlock> GetClientRatingBlocks()
        {
            var blockWebElements = Find.Elements(By.CssSelector(clientRatingBlock));
            var blocks = new List<ClientRatingBlock>();

            foreach (var block in blockWebElements)
            {
                var element = block.FindElement(By.CssSelector("div > a"));
                blocks.Add(new ClientRatingBlock() {Title = element.Text, Link = element});
            }

            return blocks;
        }

        public class ClientRatingBlock
        {
            public string Title { get; set; }
            public IWebElement Link { get; set; }
        }

        public IEnumerable<RatingChartBlock> GetRatingChartBlock()
        {
            var webElements = Find.Elements(By.CssSelector(companyRatingChartBlock)).ToList();
            var blocks = new List<RatingChartBlock>();

            foreach (var block in webElements)
            {
                var text = block.FindElement(By.CssSelector("div.company-rating-chart-inner div.heading-small")).Text;
                var element = block.FindElement(By.CssSelector("a > div"));

                blocks.Add(new RatingChartBlock() {Title = text, Link = element});
            }

            return blocks;
        }

        public class RatingChartBlock : ClientRatingBlock
        {
        }
    }
}