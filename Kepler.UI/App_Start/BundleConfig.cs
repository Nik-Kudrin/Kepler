using System.Web;
using System.Web.Optimization;

namespace Kepler
{
    public class BundleConfig
    {
       
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Content/js/jquery-{version}.js",
                        "~/Content/js/bootstrap.js",
                        "~/Content/js/jquery.metisMenu.js",
                        "~/Content/js/custom.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/css/normalize.css",
                        "~/Content/css/bootstrap.css",
                        "~/Content/css/style.css"));

        }
    }
}