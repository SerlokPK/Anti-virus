using MDMS.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MDMS.ServiceContracts
{
    [ServiceContract]
    public interface IIntrusionService
    {
        /// <summary>
        /// Adds intrusion in json fromat in file.
        /// </summary>
        /// <param name="process"></param>
        [OperationContract]
        void AddIntrusion(List<ProcessModel> process);
    }
}
