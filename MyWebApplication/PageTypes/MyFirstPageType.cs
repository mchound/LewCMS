using LewCMS.Core;
using MyWebApplication.CustomProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApplication.PageTypes
{
    [PageType(Id = "66f37878-25bb-471c-9363-d15e400b6cbf")]
    public class MyFirstPageType : Page
    {
        public string City { get; set; }
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }

        public override void OnInit()
        {
            base.OnInit();
            this.City = "Göteborg";
        }
    }
}