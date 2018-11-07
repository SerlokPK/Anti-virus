using MDMS.Common.CertificateManager;
using MDMS.ServiceContracts;
using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus
{
    public class IDSProxy: ChannelFactory<IIntrusionService>, IIntrusionService, IDisposable
    {
        IIntrusionService proxy;

        public IDSProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
						
			this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
			this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
			this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			/// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
			this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

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
