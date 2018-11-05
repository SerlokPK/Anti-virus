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
            List<ProcessModel> blackList = deserializer.Deserialize($"..\\..\\BlacklistConfig.json");

            Console.ReadKey();
        }
    }
}
