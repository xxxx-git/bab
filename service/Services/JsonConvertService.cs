using System;
using Newtonsoft.Json;
using Shared;

namespace Services {

    public class SynJsonConvertService : IJsonCovnertService
    {

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