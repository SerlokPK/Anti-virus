using MDMS.Common.CertificateManager;
using MDMS.Common.Utilities;
using MDMS.Utilities;
using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
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
				Console.Write("Option: ");
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
			/// Define the expected service certificate. It is required to establish communication using certificates.
            /// ALWAYS SERVER
			string srvCertCN = "Dalibor";
			NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
			EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9519/IntrusionService"),
									  new X509CertificateEndpointIdentity(srvCert));

			int timer = 5000;
			Int32.TryParse(_timer, out timer);

			while (true)
			{
				List<ProcessModel> blackList = ConvertJson.Deserialize($"..\\..\\BlacklistConfig.json");
				List<ProcessModel> unauthorizedProcesses = ProcessManager.CheckIfUnauthorizedExists(blackList);

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
			string user, name, startH, startM, endH, endM;
			bool userInput = false;
			Console.WriteLine("------------Create process--------------");
			do
			{
                Console.Write("[User]: ");
                Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
                user = Console.ReadLine();
				if (String.IsNullOrWhiteSpace(user))
				{
					Console.WriteLine("You have to enter valid User!");
					userInput = false;
				}
				else if (user.All(char.IsDigit))
				{
					Console.WriteLine("You have to enter at least one letter!");
					userInput = false;
				}
                else if(!regex.IsMatch(user))
                {
                    Console.WriteLine("You have to enter only letters and numbers!");
                    userInput = false;
                }
				else
				{
					userInput = true;
				}
			} while (!userInput);

			
			do
			{
                Console.Write("[Name]: ");
                Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
                name = Console.ReadLine();
				if (String.IsNullOrWhiteSpace(name))
				{
					Console.WriteLine("You have to enter valid Name!");
					userInput = false;
				}
				else if (name.All(char.IsDigit))
				{
					Console.WriteLine("You have to enter at least one letter!");
					userInput = false;
				}
                else if (!regex.IsMatch(name))
                {
                    Console.WriteLine("You have to enter only letters and numbers!");
                    userInput = false;
                }
                else
				{
					userInput = true;
				}
			} while (!userInput);
		
			do
			{
                Console.Write("[StartHours]: ");
                Regex regex = new Regex("^[0-9]+$");
                startH = Console.ReadLine();
				int temp;
				Int32.TryParse(startH, out temp);
				if (String.IsNullOrWhiteSpace(startH))
				{
					Console.WriteLine("You have to enter something!");
					userInput = false;
				}
				else if (temp < 0 || temp > 23)
				{
					Console.WriteLine("You have to enter numbers between 0 and 23!");
					userInput = false;
				}
                else if (!regex.IsMatch(startH))
                {
                    Console.WriteLine("You have to enter only numbers!");
                    userInput = false;
                }
                else
				{
					userInput = true;
				}
			} while (!userInput);

			do
			{
                Console.Write("[StartMinutes]: ");
                Regex regex = new Regex("^[0-9]+$");
                startM = Console.ReadLine();
                int temp;
				Int32.TryParse(startM, out temp);
				if (String.IsNullOrWhiteSpace(startM))
				{
					Console.WriteLine("You have to enter something!");
					userInput = false;
				}
                else if (temp < 0 || temp > 59)
                {
                    Console.WriteLine("You have to enter numbers between 0 and 59!");
                    userInput = false;
                }
                else if (!regex.IsMatch(startH))
                {
                    Console.WriteLine("You have to enter only numbers!");
                    userInput = false;
                }
				else
				{
					userInput = true;
				}
			} while (!userInput);

			do
   			{
                Console.Write("[StartMinutes]: ");
                Regex regex = new Regex("^[0-9]+$");
                endH = Console.ReadLine();
                int temp;
                Int32.TryParse(endH, out temp);
				if (String.IsNullOrWhiteSpace(endH))
				{
					Console.WriteLine("You have to enter something!");
					userInput = false;
				}
                else if (temp < 0 || temp > 23)
                {
                    Console.WriteLine("You have to enter numbers between 0 and 23!");
                    userInput = false;
                }
                else if (!regex.IsMatch(startH))
                {
                    Console.WriteLine("You have to enter only numbers!");
                    userInput = false;
                }
				else
				{
					userInput = true;
				}
			} while (!userInput);

			do
			{
                Console.Write("[EndMinutes]: ");
                Regex regex = new Regex("^[0-9]+$");
                endM = Console.ReadLine();
                int temp;
                Int32.TryParse(endM, out temp);
				if (String.IsNullOrWhiteSpace(endM))
				{
					Console.WriteLine("You have to enter something!");
					userInput = false;
				}
                else if (temp < 0 || temp > 59)
                {
                    Console.WriteLine("You have to enter numbers between 0 and 59!");
                    userInput = false;
                }
                else if (!regex.IsMatch(startH))
                {
                    Console.WriteLine("You have to enter only numbers!");
                    userInput = false;
                }
				else
				{
					userInput = true;
				}
			} while (!userInput);

			ProcessModel pm = new ProcessModel(user, name, Int32.Parse(startH), Int32.Parse(startM), Int32.Parse(endH), Int32.Parse(endM));
			return pm;
		}
	}
}
