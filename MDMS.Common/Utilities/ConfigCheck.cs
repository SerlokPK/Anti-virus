using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace MDMS.Common.Utilities
{
    public class ConfigCheck
    {
        public string createConfigHash()
        {
            //otvoriti fajl
            //sve linije uzeti
            string json = File.ReadAllText(@"..\Antivirus\BlacklistConfig.json");
            //hashirati 
            string hashCode;
            using (MD5 md5 = MD5.Create())
            {
                hashCode = BitConverter.ToString(
                  md5.ComputeHash(Encoding.UTF8.GetBytes(json))
                ).Replace("-", String.Empty);
            }

            return hashCode;
        }



    }
}
