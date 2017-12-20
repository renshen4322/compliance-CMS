using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using SEACompliance.DataBase;
using SEACompliance.DAL.Interface;
using System.Data.SqlClient;
using System.Data;
using SEACompliance.Model;
using AutoMapper;
using SEACompliance.Core.ExceptionApi;

namespace SEACompliance.DAL
{
    public class lnRIRelRecordCheckItemDataProvider : IRIRecordCheckItemDataProvider
    {
        public RecordCheckItemModel AddRecordCheckItem(RecordCheckItemModel cplnRIRelRecordCheckItem)
        {
            RecordCheckItemModel result = null;
            lnRIRelRecordCheckItem _recodeCheckItem = null;
            try
            {
                _recodeCheckItem = Mapper.Map<lnRIRelRecordCheckItem>(cplnRIRelRecordCheckItem);
                var Id = Convert.ToInt64(_recodeCheckItem.Insert());
                result = cplnRIRelRecordCheckItem;
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRelRecordCheckItemDataProvider>("Add lnRIRelRecordCheckItem Failed! \n", ex);
            }
            return result;
        }

        public bool DeleteRecordCheckItemById(string _cid,string _docid)
        {
            var query = new Sql("delete from [APlatformAppSchema].[lnRIRelRecordCheckItem] where CheckItemID=@0 AND DocID = @1 ",
                new SqlParameter { DbType = DbType.String, Value = _cid },
                new SqlParameter { DbType = DbType.String, Value = _docid });
            try
            {
                lnRIRelRecordCheckItem.repo.Execute(query);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public RecordCheckItemModel GetById(string id, string docId)
        {
            var query = new Sql("select * from [APlatformAppSchema].[lnRIRelRecordCheckItem] where CheckItemID=@0 and DocID=@1 and IsDelete=0", 
                new SqlParameter { DbType = DbType.String, Value = id },
                new SqlParameter { DbType = DbType.String, Value = docId });

            RecordCheckItemModel recordCheckItem = null;
            try
            {
                recordCheckItem = Mapper.Map<RecordCheckItemModel>(lnRIRelRecordCheckItem.SingleOrDefault(query));
            }
            catch (Exception ex)
            {

            }
            return recordCheckItem;
        }

        public RecordCheckItemModel UpdateRecordCheckItem(RecordCheckItemModel cplnRIRelRecordCheckItem)
        {
            RecordCheckItemModel result = null;
            lnRIRelRecordCheckItem _recordCheckItem = null;
            try
            {
                _recordCheckItem = Mapper.Map<lnRIRelRecordCheckItem>(cplnRIRelRecordCheckItem);
                _recordCheckItem.Update();
                result = Mapper.Map<RecordCheckItemModel>(_recordCheckItem); 
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRelRecordCheckItemDataProvider>("Update lnRIRelRecordCheckItem Failed! \n", ex);
            }
            return result;
        }

        //public RecordCheckItemModel SaveNewVersionRecordRefCheckItem(RecordCheckItemModel _et1)
        //{

        //    var query = new Sql("INSERT INTO  ",
        //        new SqlParameter { Value = _et1.LatestDocID },
        //        new SqlParameter { Value = _et1.EntityID });
        //    try
        //    {
        //        lnRIRecord.repo.Execute(query);

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error<lnRIRecordDataProvider>("UPd recordentity Failed! \n", ex);

        //    }

        //    return _et1;
        //}

        public bool DeleteRecordCheckItemById2(string _docid)
        {
            var query = new Sql(" DELETE FROM [APlatformAppSchema].[lnRICheckItem] WHERE DocID IN(SELECT CheckItemID FROM [APlatformAppSchema].[lnRIRelRecordCheckItem] WHERE DocID = @0) ",
                new SqlParameter { DbType = DbType.String, Value = _docid });
            try
            {
                lnRIRelRecordCheckItem.repo.Execute(query);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteRecordCheckItemByDocment(string _docid)
        {
            var query = new Sql(" DELETE FROM [APlatformAppSchema].[lnRIRelRecordCheckItem] WHERE DocID = @0 ",
                new SqlParameter { DbType = DbType.String, Value = _docid });
            try
            {
                lnRIRelRecordCheckItem.repo.Execute(query);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<RecordCheckItemModel> GetListCheckItemByDocId(string docId)
        {
            var query = new Sql("select * from [APlatformAppSchema].[lnRIRelRecordCheckItem] where DocID=@1 and IsDelete=0 ", new SqlParameter { DbType = DbType.String, Value = docId });
            List<RecordCheckItemModel> list = null;
            try
            {
                List<lnRIRelRecordCheckItem> riList = lnRIRelRecordCheckItem.Fetch(query);
                list = Mapper.Map<List<RecordCheckItemModel>>(riList);
            }
            catch (Exception ex)
            {
                throw new RequestErrorException(string.Format("Get docid {0} lnRIRelRecordCheckItem Failed error {1}! \n", docId, ex.Message));                
            }
            return list;
        }
    }
}
