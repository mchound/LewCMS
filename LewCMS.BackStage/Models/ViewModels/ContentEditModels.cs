using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LewCMS.BackStage.Models.ViewModels
{
    public interface IPageEditModel
    {
        IProperty Property { get; }
        object Value { get; }
        T GetValue<T>() where T : class;
    }

    public class PageEditModel : IPageEditModel
    {
        private IProperty _property;

        public IProperty Property
        {
            get { return _property; }
        }

        private object _value;

        public object Value
        {
            get { return _value; }
        }

        public PageEditModel(IProperty property, object value)
        {
            this._property = property;
            this._value = value;
        }

        public T GetValue<T>() where T : class
        {
            return _value as T;
        }
        
    }
    
}