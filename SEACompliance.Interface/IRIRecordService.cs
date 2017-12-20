using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.DataBase;
using SEACompliance.Model.RIRecordXMLModel;
using Umbraco.Core.Persistence;
using SEACompliance.Model;

namespace SEACompliance.Service.Interface
{
    public interface IlnRIRecordService: IBLLService
    {
        RIRecordModel AddRecord(RIRecordModel cplnRIRecord);
        RIRecordModel UpdateRecord(RIRecordModel cplnRIRecord);
        RIRecordModel GetById(string id);
        bool ExistsRecord(string DocID, string id);
        long GetMaxId();
        PageModel<RIRecordModel> GetRecordByKeyWordWithPage(PageModel<RIRecordModel> pageEntity, string keyWord);

        RIRecordModel AddRecord(RIRecordModel _et1, LnRIRecordEntityModel _et2);

        RIRecordModel CreateRecord(RIRecordModel cplnRIRecord);

        RIRecordModel SaveNewVersionRecord(RIRecordModel _et1);

        RIRecordModel DeleteRecord(string _documentid);
    }
}
