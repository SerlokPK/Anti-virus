using MDMS.Common.Utilities;
using MDMS.Utilities;
using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Antivirus
{
    class Program
    {
        private static readonly string _timer = ConfigurationManager.AppSettings["Timer"];
        static void Main(string[] args)
        {

            Task invoker = Task.Factory.StartNew(Checker);

            string validHash = string.Empty;
            string path = $"..\\Debug\\validHash.txt";
            List<ProcessModel> blackList = ConvertJson.Deserialize($"..\\..\\BlacklistConfig.json");

            if (!File.Exists(path))
            {
                using (StreamWriter file =File.CreateText(path)){ }
            }

            validHash = File.ReadAllText(path);

            ConfigCheck cc = new ConfigCheck();

            Task<string> task1 = Task<string>.Factory.StartNew(() => cc.createConfigHash());
            string rez = task1.Result;

            if (validHash.Equals(rez))
            {
                Console.WriteLine("Konfiguracioni fajl je sacuvanog integriteta!");
            }
            else
            {
                Console.WriteLine("Konfiguracioni fajl je korigovan van programa!");
            }

            Console.ReadKey();
        }

        private static void Checker()
        {
            List<ProcessModel> blackList = ConvertJson.Deserialize($"..\\..\\BlacklistConfig.json");
            List<ProcessModel> unauthorizedProcesses = ProcessManager.CheckIfUnauthorizedExists(blackList); 
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9519/IntrusionService";
            int timer = 5000;
            Int32.TryParse(_timer, out timer);

            while (true)
            {
                if (unauthorizedProcesses.Count > 0)
                {
                    using (IDSProxy client = new IDSProxy(binding, address))
                    {
                        client.AddIntrusion(unauthorizedProcesses);
                    }
                }

                Thread.Sleep(timer);
            }
        }
    }
}
