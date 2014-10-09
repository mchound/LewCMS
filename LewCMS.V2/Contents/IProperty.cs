﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    public interface IProperty
    {
        string Name { get; set; }
        object Get();
        void Set(object value);
        void Set(JToken jValue);
    }

    public abstract class Property : IProperty
    {
        public string Name { get; set; }

        public virtual object Get()
        {
            throw new NotImplementedException();
        }

        public virtual void Set(object value)
        {
            throw new NotImplementedException();
        }

        public virtual void Set(JToken jValue)
        {
            throw new NotImplementedException();
        }
    }
}