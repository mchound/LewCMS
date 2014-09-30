﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface IPageInfo
    {
        string Route { get; set; }
        string ParentId { get; set; }
    }

    public class PageInfo : ContentInfo, IPageInfo
    {
        public string Route { get; set; }
        public string ParentId { get; set; }

        public PageInfo(IPage page) : base(page)
        {
            this.Route = page.Route;
            this.ParentId = page.ParentId;
        }
    }
}