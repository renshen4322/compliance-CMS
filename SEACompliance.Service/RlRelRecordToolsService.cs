using SEACompliance.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.Model;
using SEACompliance.DAL.Interface;
using SEACompliance.Core.ExceptionApi;

namespace SEACompliance.Service
{
    public class RlRelRecordToolsService : IRlRelRecordToolsService
    {
        IRlRelRecordToolsProvider _IRlRelRecordToolsProvider = null;
        public RlRelRecordToolsService(IRlRelRecordToolsProvider iRlRelRecordToolsProvider)
        {
            _IRlRelRecordToolsProvider = iRlRelRecordToolsProvider;
        }
      
        public List<RIRelRecordToolModel> GetListToolsBydocId(string docId)
        {
            if (string.IsNullOrEmpty(docId))
            {
                throw new RequestErrorException("docId is null");
            }
            return _IRlRelRecordToolsProvider.GetListToolsBydocId(docId);
        }
    }
}
