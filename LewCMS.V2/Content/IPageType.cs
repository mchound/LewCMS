﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface IPageType : IContentType
    {
        string ControllerName { get; set; }
    }

    public class PageType : ContentType, IPageType
    {
        public string ControllerName { get; set; }
    }
}