using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.DataBase;
using SEACompliance.Model.RIRecordXMLModel;

namespace SEACompliance.Service.Interface
{
    public interface IRIRecordTypeService: IBLLService
    {
        List<RIRecord_Type> GetAll();
        List<RIRecord_Type> GetAll_Module();
        List<RIRecord_Type> GetSome_Topic(string _ParentCode);
        List<RIRecord_Type> GetSome_Sub(string _ParentCode);

        RIRecord_Type AddRecordType(RIRecord_Type et);
        RIRecord_Type UpdateRecordType(RIRecord_Type et);
        bool DeleteRecordType(int id);
    }
}
