using System;
using Newtonsoft.Json;
using Shared;

namespace Services {

    public class SynJsonConvertService : IJsonCovnertService
    {
        public object Deserialize(string jsonString)
        {
            var options = new JsonSerializerSettings();
            options.NullValueHandling = NullValueHandling.Ignore;
            object json = null;
            try {
                json = JsonConvert.DeserializeObject(jsonString, options);
            }
            catch (JsonException ex) {
                Console.WriteLine(ex);
            }

            return json;
        }

        public string Serialize(object json)
        {
            string stringfy = null;
            try {
                stringfy = JsonConvert.SerializeObject(json);
            }
            catch (JsonException ex) {
                Console.WriteLine(ex);
            }

            return stringfy;
        }
    }
}