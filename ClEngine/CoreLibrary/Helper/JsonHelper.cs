using System.IO;
using Newtonsoft.Json;

namespace ClEngine
{
    public static class JsonHelper
    {
        public static void SaveJson(this object type, string fileName)
        {
            var serializer = new JsonSerializer();
            using (var streamWriter = new StreamWriter(fileName))
            {
                using (var writer = new JsonTextWriter(streamWriter))
                {
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(writer, type);
                }
            }
        }

        public static object LoadJson(string fileName)
        {
            var serializer = new JsonSerializer();
            using (var streamWriter = new StreamReader(fileName))
            {
                using (var reader = new JsonTextReader(streamWriter))
                {
                    serializer.Formatting = Formatting.Indented;
                    return serializer.Deserialize(reader);
                }
            }
        }
    }
}