using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof (Kepler.UI.Startup))]

namespace Kepler.UI
{
    public partial class Startup
    {
        public static string KeplerServiceUrl = "";

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            KeplerServiceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["Kepler.Service"];
        }
    }
}