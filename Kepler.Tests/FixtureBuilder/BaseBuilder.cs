using System;

namespace Kepler.Tests.FixtureBuilder
{
    public static class BaseBuilder
    {
        private static Random random = new Random();

        public static int GetRandomId()
        {
            return random.Next(1, Int32.MaxValue);
        }
    }
}