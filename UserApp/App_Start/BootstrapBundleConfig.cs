using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace BootstrapSupport
{
    public class BootstrapBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/react").Include(
                "~/Scripts/react/react.production.min.js",
                "~/Scripts/react/react-dom.production.min"                
              ));

            bundles.Add(new ScriptBundle("~/babel").Include("~/Scripts/babel.min.js"));

            bundles.Add(new ScriptBundle("~/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-migrate-{version}.js"
             ));
            bundles.Add(new ScriptBundle("~/jqueryCookies").Include( 
                "~/Scripts/jquery.cookie.js"
             ));

            bundles.Add(new ScriptBundle("~/jquery-validation").Include(
                 "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js"
             ));

            bundles.Add(new ScriptBundle("~/bootstrap").Include(
                "~/Scripts/bootstrap.js"
            ));

            bundles.Add(new ScriptBundle("~/custom").Include(
             "~/Scripts/jquery.msgBox.js",
             "~/Scripts/jquery-ui.js",
             "~/Scripts/jquery.jtable.js",
             "~/Scripts/jquery.form.js",
             "~/Scripts/Common.js",
            "~/Scripts/bootstrap-combobox.js",
            "~/Scripts/bootstrap-typeahead.js"
            ));

            bundles.Add(new ScriptBundle("~/datepicker").Include(
                "~/Scripts/datepicker/jquery.ui.datepicker.js",
                "~/Scripts/datepicker/jquery.ui.datepicker-ar.js",
                "~/Scripts/datepicker/astro.js",
                "~/Scripts/datepicker/calendar.js"
            ));



            bundles.Add(new StyleBundle("~/content/css-en").Include(
                "~/Content/bootstrap.css",
                "~/Content/body.css",
                "~/Content/bootstrap-responsive.css"
            ));

            bundles.Add(new StyleBundle("~/content/common").Include(
                "~/Content/bootstrap-mvc-validation.css",
                "~/Content/jquery-ui.css",
                "~/Content/jtable.css",
                "~/Content/bootstrap-combobox.css",
                "~/Content/msgBoxLight.css"
                ));

        }
    }
}