using System.IO;
using System.Text;

namespace Kepler.Common.Util
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}