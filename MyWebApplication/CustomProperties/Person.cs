using LewCMS.V2;
using LewCMS.V2.Contents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApplication.CustomProperties
{
    [Property(View = "Person")]
    public class Person : Property
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Person()
        {
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Age = 0;
        }

        public override object Get()
        {
            return this;
        }

        public override void Set(object value)
        {
            Person person = value == null ? new Person() : (Person)value;

            this.FirstName = person.FirstName;
            this.LastName = person.LastName;
            this.Age = person.Age;
        }

        public override void Set(JToken jToken)
        {
            Person person = jToken.ToObject<Person>();

            this.FirstName = person.FirstName;
            this.LastName = person.LastName;
            this.Age = person.Age;
        }
    }
}