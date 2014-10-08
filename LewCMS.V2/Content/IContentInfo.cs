using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface IContentInfo : IStoreInfo
    {
        string Id { get; set; }
        string ContentTypeName { get; set; }
        string Name { get; set; }
        int Version { get; set; }
        CultureInfo Culture { get; set; }
        Type ContentType { get; }
        ContentStatus Status { get; set; }
        List<string> PropertyTypeNames { get; set; }
    }

    public abstract class ContentInfo : BaseInfo, IContentInfo
    {
        public string Id { get; set; }
        public string ContentTypeName { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public ContentStatus Status { get; set; }
        public CultureInfo Culture { get; set; }
        public List<string> PropertyTypeNames { get; set; }
        public Type ContentType { get; set; }

        public ContentInfo()
        {

        }

        public ContentInfo(IContent content)
        {
            this.Id = content.Id;
            this.ContentTypeName = content.ContentType.TypeName;
            this.PropertyTypeNames = new List<string>();
            this.Name = content.Name;
            this.Version = content.Version;
            this.Status = content.Status;
            this.Culture = content.Culture;
            this.ContentType = content.GetType();

            string propertyTypeName = string.Empty;

            foreach (var property in content.ContentType.Properties)
            {
                propertyTypeName = property.GetType().FullName;

                if (!string.IsNullOrWhiteSpace(propertyTypeName))
                {
                    this.PropertyTypeNames.Add(propertyTypeName);
                }

            }
        }

        public IEnumerable<Type> GetPropertyTypes()
        {
            Type type = null;

            foreach (var propertyTypeName in this.PropertyTypeNames)
            {
                type = Application.Current.ApplicationAssembly.GetType(propertyTypeName);

                if (type == null)
                {
                    type = Assembly.GetExecutingAssembly().GetType(propertyTypeName);
                }

                yield return type;
            }
        }

        
    }
}
