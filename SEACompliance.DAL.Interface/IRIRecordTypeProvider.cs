using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.DataBase;

namespace SEACompliance.DAL.Interface
{
    public interface IRIRecordTypeProvider: IDALProvider
    {
        List<RIRecord_Type> GetAll();

        RIRecord_Type AddRecordType(RIRecord_Type et);
        RIRecord_Type UpdateRecordType(RIRecord_Type et);
        bool DeleteRecordType(int id);
    }
}
