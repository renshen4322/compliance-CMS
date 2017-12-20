using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.DataBase;
using SEACompliance.Model;

namespace SEACompliance.DAL.Interface
{
    public interface IRICheckItemDataProvider: IDALProvider
    {
        RICheckItemModel AddCheckItem(RICheckItemModel cpRICheckItem);
        RICheckItemModel UpdateCheckItem(RICheckItemModel cpRICheckItem);
        RICheckItemModel GetById(string id);
        List<RICheckItemModel> GetCheckItemList(string id);
        bool ExistsCheckItem(string id, string DocID);
        bool DeleteCheckItemById(string id);
       long GetMaxId();

        bool DeleteCheckItemById2(string _docid);
        int GetMaxVersion(string _entityid);
        bool UpdateCheckitemOldVersionLatest(string _eid);

        void UpdateCheckItem2(RICheckItemModel _et);
        bool UpdateCheckitemOldUnderReview(string _eid);
    }
}
