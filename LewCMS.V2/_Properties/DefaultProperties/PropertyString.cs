using LewCMS.V2.Contents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Properties.DefaultProperties
{
    [Property(ViewPath = "~/BackStage/LewCMS/Views/Properties/String.cshtml")]
    public class PropertyString : Property
    {
        string _value;

        public PropertyString()
        {
            this._value = string.Empty;
        }

        public override object Get()
        {
            return this._value;
        }

        public override void Set(object value)
        {
            this._value = value == null ? string.Empty : (string)value;
        }

        public override void Set(JToken jValue)
        {
            this._value = jValue.ToObject<string>();
        }
    }
}
