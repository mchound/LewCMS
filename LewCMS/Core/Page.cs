using LewCMS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core
{
    public interface IPage
    {
        PageType PageType { get; set; }
        string Id { get; set; }
        string Route { get; set; }
        string Name { get; set; }
        string ParentId { get; set; }
        int Version { get; set; }
        object this[string propertyName] { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        ContentStatus Status { get; set; }
        IPage Clone();
        void OnInit();
    }

    public abstract class Page : IPage
    {
        public PageType PageType { get; set; }
        public string Id { get; set; }
        public string Route { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ContentStatus Status { get; set; }

        public object this[string propertyName]
        {
            get
            {
                return this.GetType().GetProperty(propertyName).GetValue(this, null);
            }
            set
            {
                this.GetType().GetProperty(propertyName).SetValue(this, value, null);
            }
        }

        public virtual void OnInit()
        {
            
        }

        public IPage Clone()
        {
            IPage clone = Activator.CreateInstance(Application.Current.ApplicationAssembly.GetType(this.PageType.TypeName)) as IPage;
            clone.PageType = this.PageType;
            clone.CreatedAt = this.CreatedAt;
            clone.Id = this.Id;
            clone.Name = this.Name;
            clone.ParentId = this.ParentId;
            clone.Route = this.Route;
            clone.Status = this.Status;
            clone.UpdatedAt = this.UpdatedAt;
            clone.Version = this.Version;

            foreach (var prop in this.PageType.Properties)
            {
                clone[prop.Name] = this[prop.Name];
            }

            return clone;
        }
    }
}
