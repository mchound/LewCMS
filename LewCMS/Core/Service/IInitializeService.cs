using LewCMS.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core.Service
{
    public interface IInitializeService
    {
        IEnumerable<IPageType> GetPageTypes(Assembly applicationAssembly);
    }

    public class InitializeService : IInitializeService
    {
        public IEnumerable<IPageType> GetPageTypes(Assembly applicationAssembly)
        {
            IEnumerable<Type> pageTypeTypes = applicationAssembly.GetTypes().Where(t => t != typeof(Page) && typeof(Page).IsAssignableFrom(t));
            List<IPageType> pageTypes = new List<IPageType>();

            foreach (Type pageType in pageTypeTypes)
            {
                PageTypeAttribute pageTypeAttribute = pageType.GetCustomAttribute<PageTypeAttribute>();
                Guid guid;
                if (pageTypeAttribute == null || string.IsNullOrWhiteSpace(pageTypeAttribute.Id) || !Guid.TryParse(pageTypeAttribute.Id, out guid))
                {
                    throw new Exception("Invalid Page Type Attribute. Id is required");
                }

                PageType _pageType = new PageType();
                _pageType.TypeName = pageType.FullName;
                _pageType.DisplayName = pageTypeAttribute.DisplayName ?? pageType.Name;
                _pageType.Id = pageTypeAttribute.Id;
                _pageType.ControllerName = string.IsNullOrWhiteSpace(pageTypeAttribute.ControllerName) ? pageType.Name : pageTypeAttribute.ControllerName;

                IEnumerable<PropertyInfo> properties = pageType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                IProperty _property;

                foreach (PropertyInfo property in properties)
                {
                    string typeName = property.PropertyType.FullName;

                    switch (typeName)
                    {
                        case "System.String":
                            _property = new PropertyString();
                            break;
                        default:
                            _property = Activator.CreateInstance(applicationAssembly.GetType(typeName)) as Property;
                            break;
                    }

                    _property.Name = property.Name;
                    _pageType.Properties.Add(_property as Property);
                }

                int pageTypeIndex = pageTypes.FindIndex(p => p.Id == _pageType.Id);

                if (pageTypeIndex > -1)
                {
                    pageTypes[pageTypeIndex] = _pageType;
                }
                else
                {
                    pageTypes.Add(_pageType);
                }

            }

            return pageTypes;
        }
    }

}
