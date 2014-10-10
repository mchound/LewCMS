using LewCMS.V2.Startup;
using LewCMS.V2.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    public interface IContentTypeCollection : IStorable
    {
        List<IPageType> PageTypes {get; set;}
        List<ISectionType> SectionTypes { get; set; }
        List<IGlobalConfigType> GlobalConfigTypes { get; set; }
        List<IContentType> ContentTypes { get; }
    }

    public class ContentTypeCollection : IContentTypeCollection, IStorable, IStoreInfo
    {
        public string Id { get; set; }
        public List<IPageType> PageTypes { get; set; }
        public List<ISectionType> SectionTypes { get; set; }
        public List<IGlobalConfigType> GlobalConfigTypes { get; set; }
        public List<IContentType> ContentTypes 
        { 
            get
            {
                return this.PageTypes.Select(pt => pt as IContentType).Concat(this.SectionTypes.Select(st => st as IContentType)).Concat(this.GlobalConfigTypes.Select(gt => gt as IContentType)).ToList();
            }
        }

        public string StoreKey
        {
            get { return "ContentTypes"; }
        }

        public string StoreDirectory
        {
            get { return "Content"; }
        }

        public IStoreInfo GetStoreInfo()
        {
            return this;
        }

        public Type RepresentedInterface
        {
            get { return typeof(IContentTypeCollection); }
        }
    }
}

