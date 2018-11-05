using MDMS.Models;
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
        [OperationContract]
        void AddIntrusion(ProcessModel process);
    }
}
