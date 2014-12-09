using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;

namespace LewCMS.BackStage.V2.Http
{
    public static class HttpExtensions
    {
        public static System.Net.Http.HttpResponseMessage StandardOkResponse(this System.Net.Http.HttpRequestMessage request, object data)
        {
            return request.CreateResponse<object>(HttpStatusCode.OK, new { success = true, data = data });
        }
    }
}