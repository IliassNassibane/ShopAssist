using Newtonsoft.Json;      // Voor de JsonTextReader Class
using Newtonsoft.Json.Linq; // Voor de Json object JObject
using System;
using System.IO;            // Voor de File object
using JsonReaderTest.Classes; // Voor de tests

namespace JsonReaderTest
{
    class Program
    {
        static void Main()
        {
            // Test 1: Vanaf een specifieke node lezen.
            SAJsonReader readjson = new SAJsonReader("JsonTest.json");
            Console.WriteLine(readjson.GetObjectFromNode("NestedJson"));

            // Test 2: Het gehele bestand lezen en in een object plaatsen.
            SAJsonReader readconfigjson = new SAJsonReader("ConfigJsonTest.json");
            SAConfiguration test =  readconfigjson.GetObjectFromJson<SAConfiguration>();
            Console.WriteLine(test.Omgeving);

            // Test 3: Een object vullen met informatie vanaf een specifieke node.
            SAJsonReader test3 = new SAJsonReader("Test3.json");
            Test3Class test3obj = test3.GetObjectFromJson<Test3Class>("Goed");
            Console.WriteLine(test3obj.uitslag);

            // Test 4: Een object wegschrijven naar zijn eigen bestand
            TestClassVoorWriter test4VoorWriter = new TestClassVoorWriter();
            test4VoorWriter.waarde = "Dit mag niet gewijzigd worden";
            test4VoorWriter.dezeVervangen = "Deze waarde is gewijzigd";
            SAJsonWriter test4 = new SAJsonWriter("Test4Bestand.json");
            test4.Write(test4VoorWriter);

            // Test 4.5: Nadat een JSON bestand is aangemaakt/aangepast, daarna een specifieke waarde lezen.
            SAJsonReader test45 = new SAJsonReader("Test4Bestand.json");
            Console.WriteLine(test45.GetObjectFromNode("dezeVervangen"));
        }
    }

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
            catch(Exception e)
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

        public T GetObjectFromJson<T>(string node="")
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
