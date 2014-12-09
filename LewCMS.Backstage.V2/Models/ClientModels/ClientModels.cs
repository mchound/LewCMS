using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LewCMS.BackStage.V2.Models.ClientModels
{
    public class PageTreeItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public bool IsStartPage { get; set; }
        public bool HasChildren { get; set; }
        public IEnumerable<PageTreeItem> Children { get; set; }
    }
}