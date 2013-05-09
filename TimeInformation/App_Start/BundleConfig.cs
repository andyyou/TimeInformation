using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TimeInformation
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region CSS
            bundles.Add(new StyleBundle("~/content/css")
                            .Include("~/Content/normalize.css")
                            .Include("~/Content/sheet.css"));

            bundles.Add(new StyleBundle("~/content/bootstrap")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/bootstrap-responsive.css")
                .Include("~/Content/font-awesome.css"));

            #endregion

            #region Javascript
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                        .Include("~/Scripts/jquery-{version}.js")
                        .Include("~/Scripts/JSLINQ.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                       "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/backbone")
                .Include("~/Scripts/underscore.js")
                .Include("~/Scripts/backbone.js")
                .Include("~/Scripts/json2.js")
                );
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datepicker")
                .Include("~/Scripts/bootstrap-datepicker.js")
                .Include("~/Scripts/bootstrap-datepicker-globalize.js"));
            #endregion
        }
    }
}