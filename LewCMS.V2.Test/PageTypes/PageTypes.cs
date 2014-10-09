using LewCMS.V2.Test.CustomProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LewCMS.V2.Contents;
using LewCMS.V2.Contents.Attributes;

namespace LewCMS.V2.Test.PageTypes
{
    [PageType(Id = "66f37878-25bb-471c-9363-d15e400b6cbf", DisplayName = "My First Page Type", ControllerName = "TheController")]
    public class MyFirstPageType : Page
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }

        public override void OnInit()
        {
            base.OnInit();

            Person1 = new Person();

            Prop1 = "Prop1_1";
            Prop2 = "Prop2_1";
            Person1.Age = 100;
            Person1.FirstName = "FirstName1";
            Person1.LastName = "LastName1";
        }
    }

    [ContentType(Id = "5464b0c5-27ce-41ec-8be9-3a95f38323d3")]
    public class MySecondPageType : Page
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string Prop3 { get; set; }
        public Person Person1 { get; set; }

        public override void OnInit()
        {
            base.OnInit();

            Person1 = new Person();

            Prop1 = "Prop1_2";
            Prop2 = "Prop2_2";
            Prop3 = "Prop3_2";
            Person1.Age = 200;
            Person1.FirstName = "FirstName2";
            Person1.LastName = "LastName2";
        }
    }
}