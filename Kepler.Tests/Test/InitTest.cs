using System.IO;
using NUnit.Framework;

namespace Kepler.Tests.Test
{
    [TestFixture]
    public class InitTest
    {
        protected static string BaseResourcePath { get; set; }

        static InitTest()
        {
            BaseResourcePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources");
        }
    }
}