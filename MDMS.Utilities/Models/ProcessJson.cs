using MDMS.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDMS.Utilities
{
    public class ProcessJson
    {
        [JsonProperty("process")]
        public ProcessModel process { get; set; }
        public void SerializeJson(string path)
        {
            string json = string.Empty;
            List<ProcessJson> blackList = new List<ProcessJson>();
            using (StreamReader r = new StreamReader(path))
            {
                json = r.ReadToEnd();
                blackList = JsonConvert.DeserializeObject<List<ProcessJson>>(json);
            }

            blackList.ForEach(x =>
            {
                string[] jsonEnd = x.process.JsonEnd.Split(':');
                string[] jsonStart = x.process.JsonStart.Split(':');

                x.process.StartHours = Int32.Parse(jsonStart[0]);
                x.process.StartMinutes = Int32.Parse(jsonStart[1]);

                x.process.EndHours = Int32.Parse(jsonEnd[0]);
                x.process.EndMinutes = Int32.Parse(jsonEnd[1]);
            });
        }


    }
}
