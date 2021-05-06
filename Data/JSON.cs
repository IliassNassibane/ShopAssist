using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ShopAssist.Data
{
    public class SAJsonReader
    {
        private JObject _JsonToRead;

        public SAJsonReader(string file)
        {
            string filePath = @".\JsonFiles\" + file;
            // TODO : Logging maken waarmee de error op geschreven kan worden.

            try
            {
                using StreamReader sr = File.OpenText(filePath);
                using JsonTextReader jr = new JsonTextReader(sr);
                _JsonToRead = (JObject)JToken.ReadFrom(jr);
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetValueFromNode(string node)
        {
            return (string)_JsonToRead[node];
        }

        public object GetObjectFromNode(string node)
        {
            return _JsonToRead[node];
        }

        public T GetObjectFromJson<T>(string node = "")
        {
            object json = _JsonToRead.ToString();
            if (node != "") json = GetObjectFromNode(node);
            return JsonConvert.DeserializeObject<T>(json.ToString());    // Misschien versimpelen door het origineel te gebruiken. Dit is alleen voor de leesbaarheid.
        }
    }

    class SAJsonWriter
    {
        private string filePath;

        public SAJsonWriter(string file)
        {
            filePath = @".\JsonFiles\" + file;
        }

        public void Write(object obj)
        {
            JObject output = JObject.FromObject(obj);
            using StreamWriter writer = File.CreateText(filePath);
            using JsonTextWriter jsonWrite = new JsonTextWriter(writer);
            output.WriteTo(jsonWrite);
        }
    }
}