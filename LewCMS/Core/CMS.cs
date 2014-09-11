using LewCMS.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace LewCMS.Core
{
    public class CMS
    {
        public List<Page> Pages { get; set; }
        public List<PageType> PageTypes { get; set; }
        public static Assembly ApplicationAssembly;

        public CMS()
        {
            this.Pages = new List<Page>();
            this.PageTypes = new List<PageType>();
        }

        public static void OnStartup(Assembly callingAssembly)
        {
            CMS.ApplicationAssembly = callingAssembly;

            CMS cms = CMS.Instance();

            if(cms == null)
            {
                cms = new CMS();
            }

            cms.Init();
            cms.Persist();
        }

        public void Init()
        {
            this.LoadPageTypes();
        }

        private void LoadPageTypes()
        {
            IEnumerable<Type> pageTypes = CMS.ApplicationAssembly.GetTypes().Where(t => t != typeof(Page) && typeof(Page).IsAssignableFrom(t));

            foreach (Type pageType in pageTypes)
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
                            _property = Activator.CreateInstance(CMS.ApplicationAssembly.GetType(typeName)) as Property;
                            break;
                    }

                    _property.Name = property.Name;
                    _pageType.Properties.Add(_property as Property);
                }

                int pageTypeIndex = this.PageTypes.FindIndex(p => p.Id == _pageType.Id);

                if (pageTypeIndex > -1)
                {
                    this.PageTypes[pageTypeIndex] = _pageType;
                }
                else
                {
                    this.PageTypes.Add(_pageType);
                }

            }
        }

        public IPage GetPageFromRoute(string route)
        {
            return this.Pages.FirstOrDefault(p => p.Route.ToLower() == route.ToLower());
        }

        public static CMS Instance()
        {
            return CMS.LoadFromCache() ?? CMS.LoadFromFile();
        }

        public void Persist()
        {
            this.SaveToCache();
            this.SaveToFile();
        }

        private void SaveToCache()
        {
            HttpRuntime.Cache["LewCMS"] = this;
        }

        private static CMS LoadFromCache()
        {
            return HttpRuntime.Cache["LewCMS"] as CMS;
        }

        private static CMS LoadFromFile()
        {
            if (File.Exists(@"C:\Temp\LewCMS.xml"))
            {
                IEnumerable<Type> types = CMS.LoadTypes(@"C:\Temp\LewCMS-PropertyTypes.xml").Concat(CMS.LoadTypes(@"C:\Temp\LewCMS-PageTypes.xml"));
                XmlSerializer serializer = new XmlSerializer(typeof(CMS), types.ToArray());
                StreamReader sr = new StreamReader(@"C:\Temp\LewCMS.xml");
                CMS cms = serializer.Deserialize(sr) as CMS;
                sr.Close();
                return cms;
            }

            return null;
        }

        private void SaveToFile()
        {
            IEnumerable<Type> propertyTypes = this.GetPropertyTypes().Distinct();
            this.SaveTypeNames(propertyTypes, @"C:\Temp\LewCMS-PropertyTypes.xml");

            IEnumerable<Type> pageTypes = this.GetPageTypes().Distinct();
            this.SaveTypeNames(pageTypes, @"C:\Temp\LewCMS-PageTypes.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(CMS), propertyTypes.Concat(pageTypes).ToArray());
            StreamWriter sw = new StreamWriter(@"C:\Temp\LewCMS.xml");
            serializer.Serialize(sw, this);
            sw.Close();
            sw.Dispose();
        }

        private IEnumerable<Type> GetPropertyTypes()
        {
            foreach (PageType pageType in this.PageTypes)
            {
                foreach (IProperty property in pageType.Properties)
                {
                    yield return property.GetType();
                }
            }
        }

        private IEnumerable<Type> GetPageTypes()
        {
            foreach (PageType pageType in this.PageTypes)
            {
                yield return pageType.GetType();
            }
        }

        private IEnumerable<string> GetTypeNames(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                yield return type.FullName;
            }
        }

        private static IEnumerable<Type> LoadTypes(string filePath)
        {
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                StreamReader sr = new StreamReader(filePath);
                IEnumerable<string> typeNames = serializer.Deserialize(sr) as List<string>;
                sr.Close();

                foreach (string typeName in typeNames)
                {
                    yield return CMS.ApplicationAssembly.GetType(typeName) ?? Type.GetType(typeName);
                }
            }
        }

        private void SaveTypeNames(IEnumerable<Type> types, string filePath)
        {
            List<string> typeNames = this.GetTypeNames(types).ToList();
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            StreamWriter sw = new StreamWriter(filePath);
            serializer.Serialize(sw, typeNames);
            sw.Close();
            sw.Dispose();
        }
    }
}
