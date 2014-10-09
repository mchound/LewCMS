using LewCMS.V2;
using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApplication.PageTypes
{
    [PageType(Id = "dd9f76ef-3e63-4a73-8170-9e84ec703b07")]
    public class MySecondPageType : Page
    {
        public string City { get; set; }
        public string Country { get; set; }

        public override void OnInit()
        {
            this.Country = "Sweden";
        }
    }
}