using MDMS.Common.Utilities;
using MDMS.ServiceContracts;
using MDMS.Utilities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace MDMS.IntrusionDetectionSystem
{
    public class IntrusionService : IIntrusionService
    {
        private readonly string _path = ConfigurationManager.AppSettings["Intrusions"];

        /// <summary>
        /// Constructor.
        /// </summary>
        public IntrusionService()
        {
        }

        /// <summary>
        /// <para>Returns tuple which contains name, user, count from forbidden processes. </para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<string, string, int>> GetIntrusions()
        {
            if (!File.Exists(_path))
                return null;

            var jsonObjects = File.ReadAllText(_path);
            List<ProcessModel> processModels = new List<ProcessModel>(ConvertJson.Deserialize(_path));

            //tuple that cointains: process name, username, count
            IEnumerable<Tuple<string, string, int>> ProcessUserCount = processModels.GroupBy(p => new { p.Name, p.User })         
                                                                                    .Select(t => new Tuple<string, string, int>(t.Key.Name, t.Key.User, t.Count()));

            return ProcessUserCount;
        }

        /// <summary>
        /// Adds intrusion in json fromat in file.
        /// </summary>
        /// <param name="process"></param>
        public void AddIntrusion(ProcessModel process)
        {
            ConvertJson.WriteToFile(process, _path);
        }
    }
}
