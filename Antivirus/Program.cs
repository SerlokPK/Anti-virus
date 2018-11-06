using MDMS.Common.Utilities;
using MDMS.Utilities;
using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Console.ReadKey();
        }

        private static void Checker()
        {
            List<ProcessModel> blackList = ConvertJson.Deserialize($"..\\..\\BlacklistConfig.json");
            List<ProcessModel> namesOfProcesses = ProcessManager.CheckIfUnauthorizedExists(blackList);

            while (true)
            {
                if (namesOfProcesses.Count > 0)
                {
                   // var foundedProcesses = blackList.FindAll(x => namesOfProcesses.Contains(x.Name));
                }

                Thread.Sleep(15000);
            }
        }
    }
}
