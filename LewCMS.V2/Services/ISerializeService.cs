using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Services
{
    public interface ISerializeService
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string serializedString) where T : class;
        T Deserialize<T>(string serializedString, Type objectType) where T : class;
    }

    public class DefaultJsonSerializer : ISerializeService
    {
        public DefaultJsonSerializer()
        {

        }

        public string Serialize<T>(T obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            return JsonConvert.SerializeObject(obj, settings);
        }

        public T Deserialize<T>(string serializedString) where T : class
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            return JsonConvert.DeserializeObject<T>(serializedString, settings) as T;
        }

        public T Deserialize<T>(string serializedString, Type objectType) where T : class
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            return JsonConvert.DeserializeObject(serializedString, objectType, settings) as T;
        }
    }
}
