using System.IO;
using Newtonsoft.Json;

namespace lde_test.Infrastructure
{
    public static class  JsonManager
    {
        public static object LoadInputJson(string input)
        {
            using (StreamReader r = new StreamReader(input))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<object>(json);
            }
        }

        public static object GetObjectValues(this object obj, string propertyName)
        {
            var type = obj.GetType();
            var property = type.GetProperty(propertyName);
            var value = property.GetValue(obj, null);

            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        public static void WriteToJsonFile(Result result , string outputFile )
        {
            // serialize JSON to a string and then write string to a file
            //File.WriteAllText(outputFile, JsonConvert.SerializeObject(result));

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(outputFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, result);
            }

        }
    }
}
