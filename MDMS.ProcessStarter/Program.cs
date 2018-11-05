using System;
using System.Collections.Generic;
using System.Linq;
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

        static List<string> Processes = new List<string>()
        {
            {"Proces1"},
            {"Proces2"},
            {"Proces3"},
            {"Proces4"}
        };

        static void Main(string[] args)
        {
            int userChoice;
            int processChoice;

            do
            {
                userChoice = userMenu();
                processChoice = processMenu();

                startProcess(userChoice, processChoice);

            } while (userChoice != 5);


            Console.ReadKey();
        }

        /// <summary>
        /// Offers a menu to choose user.
        /// </summary>
        /// <returns>
        /// Number of a user.
        /// </returns>
        static int userMenu()
        {
            int retVal = 0;

            do
            {
                Console.WriteLine("\tUser Menu:");
                Console.WriteLine("Choose an option.");
                Console.WriteLine("1.Start a process as Mario");
                Console.WriteLine("2.Start a process as Dalibor");
                Console.WriteLine("3.Start a process as Mirko");
                Console.WriteLine("4.Start a process as Strahinja");
                Console.WriteLine("5.Exit");

                retVal = Int32.Parse(Console.ReadLine());
            } while (retVal < 1 || retVal > 5);

            return retVal;
        }

        /// <summary>
        /// Offers a menu to choose process.
        /// </summary>
        /// <returns>
        /// Number of a process.
        /// </returns>
        static int processMenu()
        {
            int retVal = 0;

            do
            {
                Console.WriteLine("\tProcess Menu:");
                Console.WriteLine("Choose an option.");
                Console.WriteLine("1.Start process 1");
                Console.WriteLine("2.Start process 2");
                Console.WriteLine("3.Start process 3");
                Console.WriteLine("4.Start process 4");
                Console.WriteLine("5.Exit");

                retVal = Int32.Parse(Console.ReadLine());
            } while (retVal < 1 || retVal > 5);

            return retVal;
        }

        /// <summary>
        /// Starts a proces as a user.
        /// </summary>
        static void startProcess(int userNumber, int processNumber)
        {

        }
    }
}
