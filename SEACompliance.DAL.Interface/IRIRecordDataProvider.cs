using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.DataBase;
using Umbraco.Core.Persistence;
using SEACompliance.Model;

namespace SEACompliance.DAL.Interface
{

    public interface IlnRIRecordDataProvider : IDALProvider
    {
        RIRecordModel AddRecord(RIRecordModel cplnRIRecord);
        RIRecordModel UpdateRecord(RIRecordModel cplnRIRecord);
        RIRecordModel GetById(string id);
        bool ExistsRecord(string id, string DocID);
        long GetMaxId();
        Page<lnRIRecord> GetRecordByKeyWordWithPage(Page<lnRIRecord> pageEntity, string keyWord);

        RIRecordModel AddRecord(RIRecordModel _et1, LnRIRecordEntityModel _et2);
        bool ExistsRecordTitle(string _title);
        int GetOldVersionRecordMax(RIRecordModel _et);
        RIRecordModel UpdOldVersionRecord(RIRecordModel _et);
        LnRIRecordEntityModel UpdRecordEntity(LnRIRecordEntityModel _et1);

        void UPDOldRecordIsLast(string _eid);
        void UPDNewRecordIsLast(string _eid);
        void UPDNewRecordEntityIsLast(string _eid);

        void DelCheckItem(string _docid);
        void DelRefCheckItem(string _docid);
        List<RICheckItemModel> GetCheckItemListReadytoRef(string _docid);
    }
}
