using System.Linq;
using FluentAssertions;
using Kepler.Integration.Common.UI.Page.Insurance;

namespace Kepler.Integration.Common.UI.Validator
{
    public class InsuranceCompaniesPageValidator : BasePageValidator<InsuranceCompaniesPage>
    {
        public InsuranceCompaniesPageValidator(InsuranceCompaniesPage page)
            : base(page)
        {
        }

        public void ValiateCompaniesSortingOrder()
        {
            var companies = Page.GetCompaniesNamesFromTable()
                .Select(_ => _.GetAttribute("title").ToLower().Replace("страховая компания", "").Trim()).ToList();

            var tinkoffIndex = companies.FindIndex(_ => _ == "«тинькофф страхование»");
            var alphaStrahovanieIndex = companies.FindIndex(_ => _ == "«альфастрахование»");

            tinkoffIndex.Should().BeLessThan(alphaStrahovanieIndex, "Сортировка компаний должна работать корректно");
        }
    }
}