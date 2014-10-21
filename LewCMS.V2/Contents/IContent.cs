using LewCMS.V2.Store;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    public interface IContent : IStorable
    {
        object this[string propertyName] { get; set; }
        IContentType ContentType { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        int Version { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        ContentStatus Status { get; set; }
        CultureInfo Culture { get; set; }

        IContent Clone();
        void OnInit();
    }

    public abstract class Content : IContent
    {
        public IContentType ContentType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ContentStatus Status { get; set; }
        public CultureInfo Culture { get; set; }
        public abstract string StoreDirectory {get;}
        public virtual string StoreKey 
        {
            get
            {
                return string.Format("Content-{0}[version-{1}][lang-{2}]", this.Id, this.Version, this.Culture.TwoLetterISOLanguageName);
            }
        }

        public Content()
        {
            this.Culture = Application.Current.DefaultCulture;
        }

        public virtual object this[string propertyName]
        {
            get
            {
                PropertyInfo propertyInfo =  this.GetType().GetProperty(propertyName);
                return propertyInfo == null ? null : propertyInfo.GetValue(this, null);
            }
            set
            {
                PropertyInfo propertyInfo = this.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(this, value, null);
                }                
            }
        }

        public virtual void OnInit()
        {
            
        }

        public virtual IContent Clone()
        {
            IContent clone = Activator.CreateInstance(Application.Current.ApplicationAssembly.GetType(this.ContentType.TypeName)) as IContent;
            clone.ContentType = this.ContentType;
            clone.Id = this.Id;
            clone.Name = this.Name;
            clone.Version = this.Version;
            clone.CreatedAt = this.CreatedAt;
            clone.UpdatedAt = this.UpdatedAt;
            clone.Culture = this.Culture;
            clone.Status = this.Status;

            foreach (var prop in this.ContentType.Properties)
            {
                clone[prop.Name] = this[prop.Name];
            }

            return clone;
        }

        public abstract IStoreInfo GetStoreInfo();

    }
}
