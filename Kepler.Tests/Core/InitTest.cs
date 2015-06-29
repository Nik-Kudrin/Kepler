﻿using System.IO;
using NUnit.Framework;

namespace Kepler.Tests.Core
{
    [TestFixture]
    internal class InitTest
    {
        protected static string BasePath { get; set; }

        static InitTest()
        {
            BasePath = Directory.GetCurrentDirectory();
        }
    }
}