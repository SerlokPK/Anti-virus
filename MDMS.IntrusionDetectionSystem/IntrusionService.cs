using MDMS.Models;
using MDMS.ServiceContracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace MDMS.IntrusionDetectionSystem
{
    public class IntrusionService : IIntrusionService
    {


        public IntrusionService()
        {
        }

        private void LoadXMLFile()
        {

        }

        public void AddIntrusion(ProcessModel process)
        {
            
            JsonSerializer serializer = new JsonSerializer();

            if (!File.Exists("Intrusions"))
            {
                using (StreamWriter file = File.CreateText("Intrusions"))
                {
                    file.Write("[");
                    serializer.Serialize(file, process);
                    file.Write("]");
                }
            }
            else
            {
                var jsonObjects = File.ReadAllText("Intrusions");
                jsonObjects = jsonObjects.Remove(jsonObjects.Length - 1);
                jsonObjects += ",";
                jsonObjects += JsonConvert.SerializeObject(process);
                jsonObjects += "]";
                File.WriteAllText("Intrusions", jsonObjects);
            }
            

           
        }
    }
}
