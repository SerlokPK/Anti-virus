using MDMS.Common.CertificateManager;
using MDMS.ServiceContracts;
using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace MDMS.IntrusionDetectionSystem
{
	class IntrusionDetectionWorker
	{
		static void Main(string[] args)
		{
			/// srvCertCN.SubjectName should be set to the service's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
			NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
			string address = "net.tcp://localhost:9519/IntrusionService";

			ServiceHost host = new ServiceHost(typeof(IntrusionService));
			host.AddServiceEndpoint(typeof(IIntrusionService), binding, address);

			host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
			host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

			///PeerTrust - for development purposes only to temporarily disable the mechanism that checks the chain of trust for a certificate. 
			///To do this, set the CertificateValidationMode property to PeerTrust (PeerOrChainTrust) - specifies that the certificate can be self-issued (peer trust) 
			///To support that, the certificates created for the client and server in the personal certificates folder need to be copied in the Trusted people -> Certificates folder.
			///host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;

			///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
			host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
			host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

			///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
			host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
			host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            try
            {
                host.Open();

                Console.WriteLine("IntrusionService service is started.");
                Console.WriteLine("Press <enter> to stop service...");

                Console.ReadLine();
                host.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);

                Console.WriteLine("Press <enter> exit...");
                Console.ReadLine();
            }
		}
	}
}
