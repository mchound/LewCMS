using LewCMS.V2.Contents;
using LewCMS.V2.Contents.Attributes;
using LewCMS.V2.Properties.DefaultProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Startup
{
    public interface IInitializeService
    {
        IEnumerable<IPageType> GetPageTypes(Assembly applicationAssembly);
        IEnumerable<ISectionType> GetSectionTypes(Assembly applicationAssembly);
        IEnumerable<IGlobalConfigType> GetGlobalConfigTypes(Assembly applicationAssembly);
        IEnumerable<IContentType> GetContentTypes(Assembly applicationAssembly);
    }

    public class DefaultInitializeService : IInitializeService
    {
        public DefaultInitializeService()
        {

        }

        public IEnumerable<IContentType> GetContentTypes(Assembly applicationAssembly)
        {
            List<IContentType> contentTypes = new List<IContentType>();
            contentTypes.AddRange(this.GetPageTypes(applicationAssembly));
            contentTypes.AddRange(this.GetSectionTypes(applicationAssembly));
            contentTypes.AddRange(this.GetGlobalConfigTypes(applicationAssembly));
            return contentTypes;
        }

        public IEnumerable<IPageType> GetPageTypes(Assembly applicationAssembly)
        {
            IEnumerable<Type> pageTypeTypes = applicationAssembly.GetTypes().Where(t => t != typeof(Page) && typeof(Page).IsAssignableFrom(t));

            foreach (Type pageType in pageTypeTypes)
            {
                PageType _pageType = this.CreateContentType<PageType>(pageType, applicationAssembly);
                PageTypeAttribute pageTypeAttribute = pageType.GetCustomAttribute<PageTypeAttribute>();
                _pageType.ControllerName = pageTypeAttribute == null || string.IsNullOrWhiteSpace(pageTypeAttribute.ControllerName) ? pageType.Name : pageTypeAttribute.ControllerName;
                yield return _pageType;
            }
        }

        public IEnumerable<ISectionType> GetSectionTypes(Assembly applicationAssembly)
        {
            IEnumerable<Type> sectionTypeTypes = applicationAssembly.GetTypes().Where(t => t != typeof(Section) && typeof(Section).IsAssignableFrom(t));

            foreach (Type sectionType in sectionTypeTypes)
            {
                SectionType _sectionType = this.CreateContentType<SectionType>(sectionType, applicationAssembly);
                yield return _sectionType;
            }
        }

        public IEnumerable<IGlobalConfigType> GetGlobalConfigTypes(Assembly applicationAssembly)
        {
            IEnumerable<Type> globalConfigTypeTypes = applicationAssembly.GetTypes().Where(t => t != typeof(GlobalConfig) && typeof(GlobalConfig).IsAssignableFrom(t));

            foreach (Type globalConfigType in globalConfigTypeTypes)
            {
                GlobalConfigType _globalConfigType = this.CreateContentType<GlobalConfigType>(globalConfigType, applicationAssembly);
                yield return _globalConfigType;
            }
        }

        private T CreateContentType<T>(Type type, Assembly applicationAssembly) where T : class, new()
        {
            ContentTypeAttribute contentTypeAttribute = type.GetCustomAttribute<ContentTypeAttribute>();
            Guid contentTypeId;

            if (contentTypeAttribute == null || string.IsNullOrWhiteSpace(contentTypeAttribute.Id) || !Guid.TryParse(contentTypeAttribute.Id, out contentTypeId))
            {
                throw new Exception("Invalid Page Type Attribute. Id is required");
            }

            ContentType contentType = new T() as ContentType;
            contentType.TypeName = type.FullName;
            contentType.DisplayName = contentTypeAttribute.DisplayName ?? type.Name;
            contentType.Id = contentTypeAttribute.Id;
            contentType.Properties = this.GetContentTypeProperties(type, applicationAssembly).ToList();
            contentType.Category = contentTypeAttribute.Category;

            return contentType as T;
        }

        private IEnumerable<Property> GetContentTypeProperties(Type contentType, Assembly applicationAssembly)
        {
            IEnumerable<PropertyInfo> properties = contentType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            IProperty property;

            foreach (PropertyInfo propertyInfo in properties)
            {
                string typeName = propertyInfo.PropertyType.FullName;

                switch (typeName)
                {
                    case "System.String":
                        property = new PropertyString();
                        break;
                    default:
                        property = Activator.CreateInstance(applicationAssembly.GetType(typeName)) as Property;
                        break;
                }
                PropertyAttribute attr = property.GetType().GetCustomAttribute<PropertyAttribute>();
                property.ViewPath = attr == null ? null : attr.ViewPath;
                property.View = attr == null ? propertyInfo.Name : attr.View;
                property.Name = property.DisplayName = propertyInfo.Name;

                PropertyInfoAttribute infoAttr = propertyInfo.GetCustomAttribute<PropertyInfoAttribute>();

                if (infoAttr != null)
                {
                    property.DisplayName = infoAttr.DisplayName ?? property.Name;
                    property.Description = infoAttr.Description;
                }

                yield return property as Property;
            }
        }

    }
}
