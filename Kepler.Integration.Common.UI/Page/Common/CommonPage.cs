using NLog;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class CommonPage : BasePage
    {
        protected Logger LOG { get; set; }

        public CommonPage()
        {
            LOG = LogManager.GetLogger(this.GetType().UnderlyingSystemType.Name);
        }


        public T GetPage<T>() where T : CommonPage, new()
        {
            return GetComponent<T>();
        }
    }
}