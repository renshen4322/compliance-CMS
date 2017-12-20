using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.DataBase;
using SEACompliance.Model;

namespace SEACompliance.Service.Interface
{
    public interface IlnRIRelRecordCheckItemService: IBLLService
    {
        RecordCheckItemModel AddlnRIRelRecordCheckItem(RecordCheckItemModel cplnRIRelRecordCheckItem);
        bool DeleteRecordCheckItemById(string id,string _docid);
        RecordCheckItemModel GetById(string id, string docId);
        RecordCheckItemModel UpdateRecordCheckItem(RecordCheckItemModel cplnRIRelRecordCheckItem);
        bool DeleteRecordCheckItemByDocment(string _docid);
        List<RecordCheckItemModel> GetListCheckItemByDocId(string docId);
    }
}
