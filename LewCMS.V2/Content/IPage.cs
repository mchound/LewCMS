using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Content
{
    public interface IPage : IContent
    {
        string ParentId { get; set; }
        string Route { get; set; }
    }

    public abstract class Page : IPage
    {
        public IContentType ContentType { get; set; }
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

        public IContent Clone()
        {
            IPage clone = Activator.CreateInstance(Application.Current.ApplicationAssembly.GetType(this.ContentType.TypeName)) as IPage;
            clone.ContentType = this.ContentType;
            clone.CreatedAt = this.CreatedAt;
            clone.Id = this.Id;
            clone.Name = this.Name;
            clone.ParentId = this.ParentId;
            clone.Route = this.Route;
            clone.Status = this.Status;
            clone.UpdatedAt = this.UpdatedAt;
            clone.Version = this.Version;

            foreach (var prop in this.ContentType.Properties)
            {
                clone[prop.Name] = this[prop.Name];
            }

            return clone;
        }
    }
}
