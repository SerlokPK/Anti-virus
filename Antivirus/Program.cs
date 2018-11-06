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
        static ConfigCheck configCheck = new ConfigCheck();
        static void Main(string[] args)
        {

            Task invoker = Task.Factory.StartNew(Checker);

            while(true)
            {
                Console.WriteLine("-------MENU-------\n 1.Check blacklist configuration.\n 2.Change blacklist configuration.\n");
                var izbor = Console.ReadLine();
                switch(izbor)
                {
                    case "1":
                        ConfigurationCheck();
                        break;
                    case "2":
                        ProcessModel pm = ProcessModelCreation();
                        ConvertJson.WriteToFile(pm, $"..\\..\\BlacklistConfig.json");
                        configCheck.CreateOrUpdateConfigHash();
                        break;
                }
            }
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

        private static void ConfigurationCheck()
        {
            string validHash = string.Empty;
            string path = $"..\\Debug\\validHash.txt";
            List<ProcessModel> blackList = ConvertJson.Deserialize($"..\\..\\BlacklistConfig.json");

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path)) { }
            }

            validHash = File.ReadAllText(path);

            ConfigCheck cc = new ConfigCheck();

            if (validHash.Equals(cc.ReadConfigHash()))
            {
                Console.WriteLine("Configuration file checksum is valid!");
            }
            else
            {
                Console.WriteLine("Configuration file checksum is NOT valid!");
            }
        }

        private static ProcessModel ProcessModelCreation()
        {
            Console.WriteLine("Create process: [User] [Name] [StartHours] [StartMinutes] [EndHours] [EndMinutes]");
            string user = Console.ReadLine();
            string name = Console.ReadLine();
            string startH = Console.ReadLine(); 
            string startM = Console.ReadLine();
            string endH = Console.ReadLine();
            string endM = Console.ReadLine();
            ProcessModel pm = new ProcessModel(user, name, Int32.Parse(startH), Int32.Parse(startM), Int32.Parse(endH), Int32.Parse(endM));
            return pm;
        }
    }
}
