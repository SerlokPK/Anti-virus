using MDMS.ServiceContracts;
using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace MDMS.IntrusionDetectionSystem
{
    class IntrusionDetectionWorker
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9519/IntrusionService";

            ServiceHost host = new ServiceHost(typeof(IntrusionService));
            host.AddServiceEndpoint(typeof(IIntrusionService), binding, address);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            host.Open();

            Console.WriteLine("IntrusionService service is started.");
            Console.WriteLine("Press <enter> to stop service...");

            IntrusionService intrusion = new IntrusionService();
            intrusion.LogCriticalIntrusions();
            intrusion.AddIntrusion(new ProcessModel("daca2", "proces5", 17, 30, 18, 30));

            Console.ReadLine();
            host.Close();
        }
    }
}
