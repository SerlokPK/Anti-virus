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
        public List<ProcessModel> Deserialize(string path)
        {
            string json = string.Empty;
            List<ProcessModel> blackList = new List<ProcessModel>();
            using (StreamReader r = new StreamReader(path))
            {
                json = r.ReadToEnd();
                blackList = JsonConvert.DeserializeObject<List<ProcessModel>>(json);
            }

            return blackList;
        }


    }
}
