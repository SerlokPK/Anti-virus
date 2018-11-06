using MDMS.ServiceContracts;
using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus
{
    public class IDSProxy: ChannelFactory<IIntrusionService>, IIntrusionService, IDisposable
    {
        IIntrusionService proxy;

        public IDSProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            proxy = this.CreateChannel();
        }

        public void AddIntrusion(List<ProcessModel> process)
        {
            try
            {
                proxy.AddIntrusion(process);
            }catch(Exception e)
            {
                Trace.WriteLine($"Error in IDSProxy - {e.Message}");
            }
        }
    }
}
