using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.Service.Interface;
using SEACompliance.DAL.Interface;
using SEACompliance.DataBase;
using SEACompliance.DAL;
using SEACompliance.Model;

namespace SEACompliance.Service
{
    public class lnRICheckItemService : IlnRICheckItemService
    {
        private IRICheckItemDataProvider _lnRICheckItemDataProvider;

        public lnRICheckItemService(IRICheckItemDataProvider lnRICheckItemDataProvider)
        {
            _lnRICheckItemDataProvider = lnRICheckItemDataProvider;
        }

        

        public RICheckItemModel AddCheckItem(RICheckItemModel cplnRICheckItem)
        {
            return _lnRICheckItemDataProvider.AddCheckItem(cplnRICheckItem);
        }

        public RICheckItemModel UpdateCheckItem(RICheckItemModel cplnRICheckItem)
        {
            return _lnRICheckItemDataProvider.UpdateCheckItem(cplnRICheckItem);
        }
        public RICheckItemModel GetById(string id)
        {
            return _lnRICheckItemDataProvider.GetById(id);
        }

        public bool ExistsCheckItem(string id, string DocID)
        {
            return _lnRICheckItemDataProvider.ExistsCheckItem(id, DocID);
        }

        public List<RICheckItemModel> GetCheckItemList(string id)
        {
            return _lnRICheckItemDataProvider.GetCheckItemList(id);
        }

        public bool DeleteCheckItemById(string id)
        {
            return _lnRICheckItemDataProvider.DeleteCheckItemById(id);
        }

        public long GetMaxId()
        {
            return _lnRICheckItemDataProvider.GetMaxId();
        }

        public  int GetMaxVersion(string _entityid)
        {
            return _lnRICheckItemDataProvider.GetMaxVersion(_entityid);
        }


        public bool UpdateCheckitemOldVersionLatest(string _eid)
        {
           return _lnRICheckItemDataProvider.UpdateCheckitemOldVersionLatest(_eid);
        }

        public void UpdateCheckItem2(RICheckItemModel et)
        {
            _lnRICheckItemDataProvider.UpdateCheckItem2(et);
        }

        public bool UpdateCheckitemOldUnderReview(string _eid)
        {
            return _lnRICheckItemDataProvider.UpdateCheckitemOldUnderReview(_eid);
        }
    }
}
