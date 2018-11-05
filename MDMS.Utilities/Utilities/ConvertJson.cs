using MDMS.Utilities;
using MDMS.Utilities.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDMS.Common.Utilities
{
    public static class ConvertJson
    {
        /// <summary>
        /// Deserializes json into object(s).
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<ProcessModel> Deserialize(string path)
        {
            string json = string.Empty;
            List<ProcessModel> blackList = new List<ProcessModel>();

            if (File.Exists(path))
            {
                using (StreamReader r = new StreamReader(path))
                {
                    json = r.ReadToEnd();
                    blackList = JsonConvert.DeserializeObject<List<ProcessModel>>(json);
                }
            }

            return blackList;
        }

        /// <summary>
        /// Serializes objects into json and writes them into file.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="path"></param>
        public static void WriteToFile(ProcessModel process, string path)
        {
            JsonSerializer serializer = new JsonSerializer();

            if (!File.Exists(path))
            {
                using (StreamWriter file = File.CreateText(path))
                {
                    file.Write("[");
                    serializer.Serialize(file, process);
                    file.Write("]");
                }
            }
            else
            {
                var jsonObjects = File.ReadAllText(path);
                jsonObjects = jsonObjects.Remove(jsonObjects.Length - 1);
                jsonObjects += ",";
                jsonObjects += JsonConvert.SerializeObject(process);
                jsonObjects += "]";
                File.WriteAllText("Intrusions", jsonObjects);
            }
        }
    }
}
