using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MDMS.Utilities.Models;

namespace MDMS.Utilities.Utilities
{
    public static class IEnumerableExtensions
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static IEnumerableExtensions()
        {
            
        }

        public static IEnumerable<Tuple<string, string, int>> LogIntrusions(this IEnumerable<Tuple<string, string, int>> data)
        {
            if (data == null)
                return null;

            foreach(var tuple in data)
            {
                log.Info($"Critical: Name:{tuple.Item1}, User:{tuple.Item2}, Count:{tuple.Item3}");
            }

            return data;
        }
    }
}
