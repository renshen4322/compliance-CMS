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
using SEACompliance.Core.ExceptionApi;

namespace SEACompliance.Service
{
    public class lnRIRelRecordCheckItemService : IlnRIRelRecordCheckItemService
    {
        private IRIRecordCheckItemDataProvider _lnRIRelRecordCheckItemataProvider;

        public lnRIRelRecordCheckItemService(IRIRecordCheckItemDataProvider rRecordCheckItemDataProvider)
        {
            _lnRIRelRecordCheckItemataProvider = rRecordCheckItemDataProvider;
        }

        public RecordCheckItemModel AddlnRIRelRecordCheckItem(RecordCheckItemModel cplnRIRelRecordCheckItem)
        {
            return _lnRIRelRecordCheckItemataProvider.AddRecordCheckItem(cplnRIRelRecordCheckItem);
        }

        public bool DeleteRecordCheckItemById(string id,string _docid)
        {
            return _lnRIRelRecordCheckItemataProvider.DeleteRecordCheckItemById(id, _docid);
        }

        public RecordCheckItemModel GetById(string id, string docId)
        {
            return _lnRIRelRecordCheckItemataProvider.GetById(id, docId);
        }

        public RecordCheckItemModel UpdateRecordCheckItem(RecordCheckItemModel cplnRIRelRecordCheckItem)
        {
            return _lnRIRelRecordCheckItemataProvider.UpdateRecordCheckItem(cplnRIRelRecordCheckItem);
        }

        public bool DeleteRecordCheckItemByDocment(string _docid)
        {
            return _lnRIRelRecordCheckItemataProvider.DeleteRecordCheckItemByDocment(_docid);
        }

        public List<RecordCheckItemModel> GetListCheckItemByDocId(string docId)
        {
            if (string.IsNullOrEmpty(docId))
            {
                throw new RequestErrorException("docId is null");
            }
            return _lnRIRelRecordCheckItemataProvider.GetListCheckItemByDocId(docId);
        }
    }
}
