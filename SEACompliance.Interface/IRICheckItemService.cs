using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.DataBase;
using SEACompliance.Model;

namespace SEACompliance.Service.Interface
{
    public interface IlnRICheckItemService: IBLLService
    {
        RICheckItemModel AddCheckItem(RICheckItemModel cplnRICheckItem);
        RICheckItemModel UpdateCheckItem(RICheckItemModel cplnRICheckItem);
        RICheckItemModel GetById(string id);
        List<RICheckItemModel> GetCheckItemList(string id);
        bool ExistsCheckItem(string id, string DocID);
        bool DeleteCheckItemById(string id);
        long GetMaxId();
        int GetMaxVersion(string _entityid);
        bool UpdateCheckitemOldVersionLatest(string _eid);

        void UpdateCheckItem2(RICheckItemModel et);

        bool UpdateCheckitemOldUnderReview(string _eid);
    }
}
