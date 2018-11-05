using MDMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessJson deserializer = new ProcessJson();
            deserializer.SerializeJson($"..\\..\\BlacklistConfig.json");

            Console.ReadKey();
        }
    }
}
