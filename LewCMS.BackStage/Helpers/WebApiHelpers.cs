using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;

namespace LewCMS.BackStage.Helpers
{
    public static class WebApiHelpers
    {
        public static IEnumerable<string> ToErrorEnumerable(this System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
        }

        public static System.Net.Http.HttpResponseMessage CreateStandardOkResponse(this System.Net.Http.HttpRequestMessage request, object data)
        {
            return request.CreateResponse<object>(HttpStatusCode.OK, new { success = true, data = data });
        }

        public static System.Net.Http.HttpResponseMessage CreateStandardErrorResponse(this System.Net.Http.HttpRequestMessage request, System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            return request.CreateStandardErrorResponse(modelState.ToErrorEnumerable());
        }

        public static System.Net.Http.HttpResponseMessage CreateStandardErrorResponse(this System.Net.Http.HttpRequestMessage request, IEnumerable<string> errors)
        {
            return request.CreateResponse<object>(HttpStatusCode.OK, new { success = false, errorMessages = errors });
        }

        public static System.Net.Http.HttpResponseMessage CreateStandardErrorResponse(this System.Net.Http.HttpRequestMessage request, string error)
        {
            return request.CreateStandardErrorResponse(new string[] {error});
        }
    }
}