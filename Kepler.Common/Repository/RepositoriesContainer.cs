namespace Kepler.Common.Repository
{
    public class RepositoriesContainer
    {
        public TestAssemblyRepository AssemblyRepo { get; set; }
        public TestSuiteRepository SuiteRepo { get; set; }
        public TestCaseRepository CaseRepo { get; set; }
        public ScreenShotRepository ScreenShotRepo { get; set; }

        public RepositoriesContainer()
        {
            AssemblyRepo = TestAssemblyRepository.Instance;
            SuiteRepo = TestSuiteRepository.Instance;
            CaseRepo = TestCaseRepository.Instance;
            ScreenShotRepo = ScreenShotRepository.Instance;
        }
    }
}