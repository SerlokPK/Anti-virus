using log4net;
using MDMS.Common.Utilities;
using MDMS.ServiceContracts;
using MDMS.Utilities.Models;
using MDMS.Utilities.Utilities;
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
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _path = ConfigurationManager.AppSettings["Intrusions"];
        private int _maxIntrusionsAllowed = Int32.Parse(ConfigurationManager.AppSettings["MaxIntrusionNumber"]);

        /// <summary>
        /// Constructor.
        /// </summary>
        public IntrusionService()
        {
        }

        /// <summary>
        /// Adds intrusion in json fromat in file.
        /// </summary>
        /// <param name="process"></param>
        public void AddIntrusion(List<ProcessModel> process)
        {
            Logger.Info($"Adding intrusions to file.");

            try
            {
                process.ForEach(x => ConvertJson.WriteToFile(x, _path));
                LogCriticalIntrusions();
            }
            catch (Exception e)
            {
                Logger.Error($"Adding intrusions to file failed with error:{e}");
            }
        }

        private void LogCriticalIntrusions()
        {
            Logger.Info($"Logging critical intrusions.");
            try
            {
                IEnumerable<Tuple<string, string, int>> intrusions = GetIntrusions();

                if (intrusions == null)
                    return;

                //if needed later to return critical intrusions to AV service
                var elements = intrusions.ToList().Where(x => x.Item3 >= _maxIntrusionsAllowed).LogIntrusions();
            }
            catch (Exception e)
            {
                Logger.Error($"Logging critical intrusions failed with error:{e}");
            }
        }

        private IEnumerable<Tuple<string, string, int>> GetIntrusions()
        {
            if (!File.Exists(_path))
                throw new Exception($"File:{_path} doesnt exsist");

            var jsonObjects = File.ReadAllText(_path);
            List<ProcessModel> processModels = new List<ProcessModel>(ConvertJson.Deserialize(_path));

            //tuple that cointains: process name, username, count
            IEnumerable<Tuple<string, string, int>> ProcessUserCount = processModels.GroupBy(p => new { p.Name, p.User })
                                                                                    .Select(t => new Tuple<string, string, int>(t.Key.Name, t.Key.User, t.Count()));

            return ProcessUserCount;
        }
    }
}
