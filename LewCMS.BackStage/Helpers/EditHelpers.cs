using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LewCMS.BackStage.Helpers
{
    public static class EditHelpers
    {
        public static IEnumerable<string> ClientScriptPaths(this IContent content)
        {
            HashSet<string> scriptPaths = new HashSet<string>();
            string src = string.Empty;

            foreach (var prop in content.ContentType.Properties)
            {
                src = prop.ClientScriptPath ?? string.Format("/ClientScripts/{0}.js", prop.ClientScript);
                if (!scriptPaths.Any(s => s == src))
                {
                    scriptPaths.Add(src);
                }
            }

            return scriptPaths;

        }
    }
}