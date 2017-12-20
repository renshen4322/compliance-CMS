using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.DataBase;
using SEACompliance.Model;

namespace SEACompliance.DAL.Interface
{
    public interface IRIRecordCheckItemDataProvider: IDALProvider
    {
        RecordCheckItemModel AddRecordCheckItem(RecordCheckItemModel cpRIRecordCheckItem);
        bool DeleteRecordCheckItemById(string id, string docId);
        RecordCheckItemModel GetById(string id, string docId);
        RecordCheckItemModel UpdateRecordCheckItem(RecordCheckItemModel cpRIRecordCheckItem);
        bool DeleteRecordCheckItemById2(string _docid);
        bool DeleteRecordCheckItemByDocment(string _docid);
        List<RecordCheckItemModel> GetListCheckItemByDocId(string docId);
    }
}
