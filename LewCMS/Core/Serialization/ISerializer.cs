using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core.Serialization
{
    public interface ISerializer
    {
        void SerializeToFile<T>(T obj, string filePath);
        T DeserializeFromFile<T>(string filePath);
        T DeserializeFromFile<T>(string filePath, Type objectType) where T : class;
    }

    public class LewCMSJsonSerializer : ISerializer
    {
        public void SerializeToFile<T>(T obj, string filePath)
        {
            StreamWriter sw = new StreamWriter(filePath);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            sw.Write(JsonConvert.SerializeObject(obj, settings));
            sw.Close();
            sw.Dispose();
        }

        public T DeserializeFromFile<T>(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            T obj =  JsonConvert.DeserializeObject<T>(sr.ReadToEnd(), settings);
            sr.Close();
            return obj;
        }

        public T DeserializeFromFile<T>(string filePath, Type objectType) where T : class
        {
            StreamReader sr = new StreamReader(filePath);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            T obj = JsonConvert.DeserializeObject(sr.ReadToEnd(), objectType, settings) as T;
            sr.Close();
            return obj;
        }
    }
}
