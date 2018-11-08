using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus
{
    public class ProcessManager
    {
        /// <summary>
        /// Checks if there is active unauthorized process in list of processes
        /// </summary>
        /// <returns></returns>
        public static List<ProcessModel> CheckIfUnauthorizedExists(List<ProcessModel> blackList)
        {
            var processes = Process.GetProcesses().ToList();
                                                                                                                            //if we have greater hour, we don't need to check minutes, otherwise, we need to check minutes,
                                                                                                                            //it's all done because of zero
            var unauthorized = processes.SelectMany(x => blackList.FindAll(y => y.Name == x.ProcessName && y.User == GetProcessOwner(x.Id) && ((y.StartHours < DateTime.Now.Hour) || (y.StartHours == DateTime.Now.Hour && y.StartMinutes <= DateTime.Now.Minute))
                                                                                                                && ((y.EndHours > DateTime.Now.Hour) || (y.EndHours == DateTime.Now.Hour && y.EndMinutes >= DateTime.Now.Minute)))).ToList();

            return unauthorized;
        }


        public static string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectCollection calledProcess = new ManagementObjectSearcher(query).Get();

            foreach (ManagementObject obj in calledProcess)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[0];
                }
            }

            return "NO OWNER";
        }
    }
}
