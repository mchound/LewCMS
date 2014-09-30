using LewCMS.V2.Content;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface IContent
    {
        object this[string propertyName] { get; set; }
        IContentType ContentType { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        int Version { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        ContentStatus Status { get; set; }

        IContent Clone();
        void OnInit();
    }
}
