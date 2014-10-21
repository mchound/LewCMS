using LewCMS.V2;
using LewCMS.V2.Contents;
using MyWebApplication.CustomProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApplication.PageTypes
{
    [PageType(Id = "66f37878-25bb-471c-9363-d15e400b6cbf", Category = "Standard", DisplayName = "My First")]
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

    [PageType(Id = "6B43064D-2E8E-4591-925B-6339CFA0943D", Category = "Standard", DisplayName = "My Third Page")]
    public class MyThirdPageType : Page
    {
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }

        public override void OnInit()
        {
            this.String1 = "Value of string 1";
            this.String2 = "Value of string 2";
            this.String3 = "Value of string 3";
        }
    }

    [PageType(Id = "85451C53-28C1-44FD-9399-DE20FE27A700")]
    public class FourthPageType : Page
    {
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
    }
}