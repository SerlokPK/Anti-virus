using MDMS.Common.Utilities;
using MDMS.Utilities;
using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9519/IntrusionService";
            Task invoker = Task.Factory.StartNew(Checker);

            

            string validHash = string.Empty;
            string path = $"..\\Debug\\validHash.txt";
            List<ProcessModel> blackList = ConvertJson.Deserialize($"..\\..\\BlacklistConfig.json");

            if (!File.Exists(path))
            {
                File.Create(path);
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
            List<ProcessModel> namesOfProcesses = ProcessManager.CheckIfUnauthorizedExists(blackList);

            while (true)
            {
                Console.WriteLine("usao je i ovde");
                if (namesOfProcesses.Count > 0)
                {
                   // var foundedProcesses = blackList.FindAll(x => namesOfProcesses.Contains(x.Name));
                }

                Thread.Sleep(15000);
            }
        }
    }
}
