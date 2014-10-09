using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    public interface IContentType
    {
        string Id { get; set; }
        string DisplayName { get; set; }
        string TypeName { get; set; }
        List<Property> Properties { get; set; }
        IContent CreateInstance(string name);
        T CreateInstance<T>(string name) where T : class, IContent;
    }

    public abstract class ContentType : IContentType
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public List<Property> Properties { get; set; }
        public string TypeName { get; set; }

        public ContentType()
        {
            this.Properties = new List<Property>();
        }


        public virtual IContent CreateInstance(string name)
        {
            IContent content = Activator.CreateInstance(Application.Current.ApplicationAssembly.GetType(this.TypeName)) as IContent;
            content.Name = name;
            content.Id = Guid.NewGuid().ToString();
            content.Version = 1;
            content.ContentType = this;
            content.CreatedAt = DateTime.Now;
            content.UpdatedAt = content.CreatedAt;
            content.Culture = Application.Current.DefaultCulture;

            content.OnInit();

            return content;
        }

        public virtual T CreateInstance<T>(string name) where T : class, IContent
        {
            return this.CreateInstance(name) as T;
        }
    }
}
