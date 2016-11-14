using System;

namespace Sravni.ScreenShotTest.Screens
{
    [TestSuite]
    public class ActionsForUrls
    {
        [Url("/bank/skb-bank/kredit/na-vsjo-pro-vsjo/?amount=500000cy=1&period=3+%D0%B3%D0%BE%D0%B4%D0%B0")]
        [Test]
        public void Url1Actions()
        {
        }

        public void Url2Actions()
        {
        }
    }

    public class TestSuiteAttribute : Attribute
    {
    }

    public class TestAttribute : Attribute
    {
    }

    public class Url : System.Attribute
    {
        private string _name;
        public double Version;

        public Url(string name)
        {
            _name = name;
            Version = 1.0;
        }
    }
}