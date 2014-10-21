using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;

namespace LewCMS.BackStage.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString EditForProperty(this HtmlHelper helper, IProperty property, IPage page)
        {
            string viewPath = property.ViewPath ?? string.Format("~/Shared/Properties/{0}.cshtml", property.View);
            
            ViewDataDictionary viewData = new ViewDataDictionary(page[property.Name]);
            viewData.Add("PropertyName", property.Name);
            return helper.Partial(viewPath, viewData);
        }

        public static MvcHtmlString ClientScriptsForProperties(this HtmlHelper helper, IEnumerable<IProperty> properties)
        {
            TagBuilder script = null;
            string src = string.Empty;
            string html = string.Empty;
            List<string> scriptPaths = new List<string>();

            foreach (var prop in properties)
            {
                src = prop.ClientScriptPath ?? string.Format("/ClientScripts/{0}.js", prop.ClientScript);
                if (!scriptPaths.Exists(s => s == src))
                {
                    scriptPaths.Add(src);
                }
            }

            foreach (var _src in scriptPaths)
            {
                script = new TagBuilder("script");
                script.Attributes.Add("type", "text/javascript");
                script.Attributes.Add("src", _src);
                html = string.Concat(html, script.ToString());
            }

            return MvcHtmlString.Create(html);
        }
    }
}