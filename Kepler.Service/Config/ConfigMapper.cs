using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Kepler.Common.Models;

namespace Kepler.Service.Config
{
    public class ConfigMapper
    {
        public Project GetProject(ImportConfigModel.ProjectConfig projectConfig)
        {
            Mapper.Reset();

            Mapper.Configuration.AllowNullCollections = true;
            Mapper.Configuration.AllowNullDestinationValues = true;

            Mapper.CreateMap<ImportConfigModel.ProjectConfig, Project>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Branches, opt => opt.Ignore())
                .ForMember(dest => dest.MainBranchId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();

            return Mapper.Map<ImportConfigModel.ProjectConfig, Project>(projectConfig);
        }

        public IEnumerable<Project> GetProjects(IEnumerable<ImportConfigModel.ProjectConfig> projectsConfig)
        {
            return projectsConfig.Select(config => GetProject(config));
        }

        public Branch GetBranch(ImportConfigModel.BranchConfig branchConfig)
        {
            Mapper.Reset();

            Mapper.Configuration.AllowNullCollections = true;
            Mapper.Configuration.AllowNullDestinationValues = true;

            Mapper.CreateMap<ImportConfigModel.BranchConfig, Branch>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BaseLineId, opt => opt.Ignore())
                .ForMember(dest => dest.Builds, opt => opt.Ignore())
                .ForMember(dest => dest.IsMainBranch, opt => opt.Ignore())
                .ForMember(dest => dest.LatestBuildId, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();

            return Mapper.Map<ImportConfigModel.BranchConfig, Branch>(branchConfig);
        }

        public IEnumerable<Branch> GetBranches(IEnumerable<ImportConfigModel.BranchConfig> branchesConfig)
        {
            return branchesConfig.Select(config => GetBranch(config));
        }

        public TestAssembly GetAssembly(ImportConfigModel.TestAssemblyConfig assemblyConfig)
        {
            Mapper.Reset();

            Mapper.Configuration.AllowNullCollections = true;
            Mapper.Configuration.AllowNullDestinationValues = true;

            Mapper.CreateMap<ImportConfigModel.TestAssemblyConfig, TestAssembly>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TestSuites, opt => opt.Ignore())
                .ForMember(dest => dest.BuildId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ParentObjId, opt => opt.Ignore());


            Mapper.AssertConfigurationIsValid();

            return Mapper.Map<ImportConfigModel.TestAssemblyConfig, TestAssembly>(assemblyConfig);
        }

        public IEnumerable<TestAssembly> GetAssemblies(
            IEnumerable<ImportConfigModel.TestAssemblyConfig> testAssemblyConfigs)
        {
            return testAssemblyConfigs.Select(config => GetAssembly(config));
        }

        public TestSuite GetSuite(ImportConfigModel.TestSuiteConfig suiteConfig)
        {
            Mapper.Reset();

            Mapper.Configuration.AllowNullCollections = true;
            Mapper.Configuration.AllowNullDestinationValues = true;

            Mapper.CreateMap<ImportConfigModel.TestSuiteConfig, TestSuite>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.TestCases, opt => opt.Ignore())
                .ForMember(dest => dest.BuildId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ParentObjId, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();

            return Mapper.Map<ImportConfigModel.TestSuiteConfig, TestSuite>(suiteConfig);
        }

        public IEnumerable<TestSuite> GetSuites(IEnumerable<ImportConfigModel.TestSuiteConfig> suiteConfigs)
        {
            return suiteConfigs.Select(config => GetSuite(config));
        }

        public TestCase GetCase(ImportConfigModel.TestCaseConfig caseConfig)
        {
            Mapper.Reset();

            Mapper.Configuration.AllowNullCollections = true;
            Mapper.Configuration.AllowNullDestinationValues = true;

            Mapper.CreateMap<ImportConfigModel.TestCaseConfig, TestCase>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ScreenShots, opt => opt.Ignore())
                .ForMember(dest => dest.BuildId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ParentObjId, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();

            return Mapper.Map<ImportConfigModel.TestCaseConfig, TestCase>(caseConfig);
        }

        public IEnumerable<TestCase> GetCases(IEnumerable<ImportConfigModel.TestCaseConfig> testCaseConfigs)
        {
            return testCaseConfigs.Select(config => GetCase(config));
        }
    }
}