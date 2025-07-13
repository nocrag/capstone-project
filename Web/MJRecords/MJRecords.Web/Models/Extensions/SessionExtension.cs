using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace MJRecords.Web.Models
{
    public static class SessionExtension
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            // JsonSerializerOptions to handle potential circular references
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                MaxDepth = 64 
            };

            string serializedData = JsonSerializer.Serialize(value, options);
            session.SetString(key, serializedData);
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (string.IsNullOrEmpty(data))
            {
                return default;
            }

            // JsonSerializerOptions to handle potential circular references
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                MaxDepth = 64 
            };

            try
            {
                return JsonSerializer.Deserialize<T>(data, options);
            }
            catch
            {
                // If deserialization fails, return default
                return default;
            }
        }
    }
}
