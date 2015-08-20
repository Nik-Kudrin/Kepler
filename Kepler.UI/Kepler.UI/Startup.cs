using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kepler.UI.Startup))]
namespace Kepler.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
