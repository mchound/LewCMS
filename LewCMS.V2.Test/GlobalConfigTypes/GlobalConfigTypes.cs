using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LewCMS.V2.Contents;
using LewCMS.V2.Contents.Attributes;
using LewCMS.V2.Test.CustomProperties;

namespace LewCMS.V2.Test.SectionTypes
{
    [ContentType(Id = "2a5314ac-0065-4ef3-bb43-7023e24afdf6", DisplayName = "My First Global Config Type")]
    public class MyFirstGlobalConfigType : GlobalConfig
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string Prop3 { get; set; }
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }

        public override void OnInit()
        {
            base.OnInit();

            Person1 = new Person();

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

    [ContentType(Id = "1f620ebb-2ff5-40e8-acb0-d8507f323e54")]
    public class MySecondGlobalConfigType : GlobalConfig
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string Prop3 { get; set; }
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }

        public override void OnInit()
        {
            base.OnInit();

            Person1 = new Person();

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
