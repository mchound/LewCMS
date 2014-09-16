﻿using Newtonsoft.Json;
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
    }

    public class PageMetaData
    {
        public string PageId { get; set; }
        public string PageTypeName { get; set; }
        public List<string> PropertyTypeNames { get; set; }
        public string PageRoute { get; set; }
        public string ParentId { get; set; }
        public string PageName { get; set; }
        public int Version { get; set; }
        public ContentStatus Status { get; set; }

        public PageMetaData()
        {

        }

        public PageMetaData(IPage page)
        {
            this.PageId = page.Id;
            this.PageTypeName = page.PageType.TypeName;
            this.PropertyTypeNames = new List<string>();
            this.PageRoute = page.Route;
            this.ParentId = page.ParentId;
            this.PageName = page.Name;
            this.Version = page.Version;
            this.Status = page.Status;

            string propertyTypeName = string.Empty;

            foreach (var property in page.PageType.Properties)
            {
                propertyTypeName = property.GetType().FullName;

                if(!string.IsNullOrWhiteSpace(propertyTypeName))
                {
                    this.PropertyTypeNames.Add(propertyTypeName);
                }
                
            }
        }

        public Type GetPageInstanceType()
        {
            return Application.Current.ApplicationAssembly.GetType(this.PageTypeName);
        }

        public IEnumerable<Type> GetPropertyTypes()
        {
            Type type = null;

            foreach (var propertyTypeName in this.PropertyTypeNames)
            {
                type = Application.Current.ApplicationAssembly.GetType(propertyTypeName);

                if(type == null)
                {
                    type = Assembly.GetExecutingAssembly().GetType(propertyTypeName);
                }

                yield return type;
            }
        }
    }
}
