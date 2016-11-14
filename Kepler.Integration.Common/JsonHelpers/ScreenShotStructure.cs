using System;
using System.Collections.Generic;
using System.Reflection;
using Kepler.Integration.Common.Screens;

namespace Kepler.Integration.Common.JsonHelpers
{
    public class ScreenShot
    {
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class TestCase
    {
        public TaskUsageStatus UsedStatus { get; set; }
        public Type TestSuiteType { get; set; }
        public MethodInfo Method { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public List<ScreenShot> ScreenShots { get; set; }
    }

    public class TestSuite
    {
        public string Name { get; set; }
        public List<TestCase> TestCases { get; set; }
    }

    public class TestAssembly
    {
        public string Name { get; set; }
        public List<TestSuite> TestSuites { get; set; }
    }

    public class Branch
    {
        public string Name { get; set; }
        public List<TestAssembly> TestAssemblies { get; set; }
    }

    public class Project
    {
        public string Name { get; set; }
        public List<Branch> Branches { get; set; }
    }

    public class ScreenShotStructure
    {
        public List<Project> Projects { get; set; }
    }
}