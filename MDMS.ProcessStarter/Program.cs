using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MDMS.ProcessStarter
{
    class Program
    {
        static Dictionary<string, string> Users = new Dictionary<string, string>()
        {
            {"Mario", "mario123"},
            {"Dalibor", "dalibor123"},
            {"Mirko", "mirko123"},
            {"Strahinja", "strahinja123"}
        };

        static Dictionary<string, string> Processes = new Dictionary<string, string>()
        {
            {"Notepad", @"C:\Windows\system32\notepad.exe"},
            {"Chrome", @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"},
            {"Mozzila", @"C:\Program Files\Mozilla Firefox\firefox.exe"},
            {"Paint", @"C:\Windows\system32\paint.exe"}
        };

        static void Main(string[] args)
        {
            createUsers();

            string userChoice;
            string processChoice;

            do
            {
                userChoice = userMenu();

                if (userChoice == "EXIT")
                    break;

                processChoice = processMenu();

                startProcess(userChoice, processChoice);

            } while (true);


            removeUsers();

            Console.ReadKey();
        }

        /// <summary>
        /// Offers a menu to choose user.
        /// </summary>
        /// <returns>
        /// Name of a user.
        /// </returns>
        static string userMenu()
        {
            string retVal = "";
            string read = "";
            int temp = 0;

            do
            {
                Console.WriteLine("\tUser Menu:");
                Console.WriteLine("Choose an option.");
                Console.WriteLine("1.Start a process as Mario");
                Console.WriteLine("2.Start a process as Dalibor");
                Console.WriteLine("3.Start a process as Mirko");
                Console.WriteLine("4.Start a process as Strahinja");
                Console.WriteLine("5.Exit");
                Console.Write("Option: ");
                read = Console.ReadLine();

                Int32.TryParse(read, out temp);
            } while (temp < 1 || temp > 5);

            switch(temp)
            {
                case 1:
                    retVal = "Mario";
                    break;
                case 2:
                    retVal = "Dalibor";
                    break;
                case 3:
                    retVal = "Mirko";
                    break;
                case 4:
                    retVal = "Strahinja";
                    break;
                case 5:
                    retVal = "EXIT";
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Offers a menu to choose process.
        /// </summary>
        /// <returns>
        /// Name of a process.
        /// </returns>
        static string processMenu()
        {
            string retVal = "";
            string read = "";
            int temp = 0;

            do
            {
                Console.WriteLine("\tProcess Menu:");
                Console.WriteLine("Choose an option.");
                Console.WriteLine("1.Start Notepad");
                Console.WriteLine("2.Start Google Chrome");
                Console.WriteLine("3.Start Mozzila Firefox");
                Console.WriteLine("4.Start Paint");
                Console.Write("Option: ");
                read = Console.ReadLine();

                Int32.TryParse(read, out temp);
            } while (temp < 1 || temp > 4);

            switch (temp)
            {
                case 1:
                    retVal = "Notepad";
                    break;
                case 2:
                    retVal = "Chrome";
                    break;
                case 3:
                    retVal = "Mozzila";
                    break;
                case 4:
                    retVal = "Paint";
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Starts a proces as a user.
        /// </summary>
        static void startProcess(string user, string process)
        {
            SecureString pass = new SecureString();
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.UserName = user;
            p.StartInfo.FileName = Processes[process];
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(Processes[process]);
            foreach(char c in Users[user])
            {
                pass.AppendChar(c);
            }
            p.StartInfo.Password = pass;
            p.Start();
        }

        /// <summary>
        /// Creates users.
        /// </summary>
        static void createUsers()
        {
            foreach(KeyValuePair<string, string> kp in Users)
            {
                try
                {
                    DirectoryEntry directoryEntries = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                    DirectoryEntry newUser = directoryEntries.Children.Add(kp.Key, "user");
                    newUser.Invoke("SetPassword", new object[] { kp.Value });
                    newUser.Invoke("Put", new object[] { "Description", "User: \"" + kp.Key + "\" for MDMS" });
                    newUser.CommitChanges();

                    DirectoryEntry group;
                    group = directoryEntries.Children.Find("Guests", "group");
                    if (group != null)
                    {
                        group.Invoke("Add", new object[] { newUser.Path.ToString() });
                    }
                    Console.WriteLine("Account: \"" + kp.Key + "\" created successfully.");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Removes created users.
        /// </summary>
        static void removeUsers()
        {
            foreach (KeyValuePair<string, string> kp in Users)
            {
                try
                {
                    DirectoryEntry directoryEntries = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                    DirectoryEntry user = directoryEntries.Children.Find(kp.Key);
                    directoryEntries.Children.Remove(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
