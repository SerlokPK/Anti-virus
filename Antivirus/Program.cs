using MDMS.Common.Utilities;
using MDMS.Utilities;
using MDMS.Utilities.Models;
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
            List<ProcessModel> blackList = ConvertJson.Deserialize($"..\\..\\BlacklistConfig.json");

            Console.ReadKey();
        }
    }
}
