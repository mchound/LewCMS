using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;
using LewCMS.BackStage.Models.ViewModels;

namespace LewCMS.BackStage.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString EditForProperty(this HtmlHelper helper, IProperty property, IPage page)
        {
            string viewPath = property.ViewPath ?? string.Format("~/Shared/Properties/{0}.cshtml", property.View);

            //ViewDataDictionary viewData = new ViewDataDictionary<PageEditModel>(new PageEditModel { Property = property, Value = page[property.Name] });
            //viewData.Add("PropertyName", property.Name);
            //viewData.Add("ClientValidation", property.ClientValidationNotation);
            return helper.Partial(viewPath, new PageEditModel(property, page[property.Name]));
        }
    }
}