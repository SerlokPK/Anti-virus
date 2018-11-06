using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Antivirus
{
    public class ProcessManager
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
        /// <summary>
        /// Checks if there is active unauthorized process in list of processes
        /// </summary>
        /// <returns></returns>
        public static List<ProcessModel> CheckIfUnauthorizedExists(List<ProcessModel> blackList)
        {
            var processes = Process.GetProcesses().ToList();

            var unauthorized = processes.SelectMany(x => blackList.FindAll(y => y.Name == x.ProcessName && y.User == GetProcessUser(x))).ToList();

            return unauthorized;
        }

        private static string GetProcessUser(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                WindowsIdentity wi = new WindowsIdentity(processHandle);
                string user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }
    }
}
