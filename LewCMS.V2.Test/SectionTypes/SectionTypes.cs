using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LewCMS.V2.Attributes;
using LewCMS.V2.Test.CustomProperties;

namespace LewCMS.V2.Test.SectionTypes
{
    [ContentType(Id = "b4e1b19c-6eed-4426-bbd3-cb305d84903f", DisplayName = "My First Section Type")]
    public class MyFirstSectionType : Section
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string Prop3 { get; set; }
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }

        public override void OnInit()
        {
            base.OnInit();

            Prop1 = "Prop1";
            Prop2 = "Prop2";
            Prop3 = "Prop3";
            Person1.Age = 100;
            Person1.FirstName = "Firstname1";
            Person1.LastName = "Lastname1";

            Person1.Age = 200;
            Person1.FirstName = "Firstname2";
            Person1.LastName = "Lastname2";
        }
    }

    [ContentType(Id = "fa9a1819-a6ed-43c1-bd70-e3a91a6924ec", DisplayName = "My Second Section Type")]
    public class MySecondSectionType : Section
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string Prop3 { get; set; }
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }

        public override void OnInit()
        {
            base.OnInit();

            Prop1 = "Prop1_2";
            Prop2 = "Prop2_2";
            Prop3 = "Prop3_3";
            Person1.Age = 1000;
            Person1.FirstName = "Firstname1_2";
            Person1.LastName = "Lastname1_2";

            Person1.Age = 2000;
            Person1.FirstName = "Firstname2_2";
            Person1.LastName = "Lastname2_2";
        }
    }
}
