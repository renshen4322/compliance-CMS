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

namespace SEACompliance.DAL
{
    public class lnRIRecordDataProvider : IlnRIRecordDataProvider
    {
        

        public RIRecordModel AddRecord(RIRecordModel cplnRIRecord)
        {
            RIRecordModel result = null;
            lnRIRecord _record = null;
            try
            {
                _record = Mapper.Map<lnRIRecord>(cplnRIRecord);
                var recordId = Convert.ToInt64(_record.Insert());
                result = cplnRIRecord;
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordDataProvider>("Add lnRIRecord Failed! \n", ex);
            }
            return result;
        }

        public RIRecordModel UpdateRecord(RIRecordModel cplnRIRecord)
        {
            RIRecordModel result = null;
            lnRIRecord _record = null;
            try
            {
                _record = Mapper.Map<lnRIRecord>(cplnRIRecord);
                _record.Update();

                result = Mapper.Map<RIRecordModel>(_record);
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordDataProvider>("Update lnRIRecord Failed! \n", ex);
            }
            return result;
        }

        public RIRecordModel GetById(string id)
        {

            var query = new Sql("select * from [APlatformAppSchema].[lnRIRecord] where DocID=@0 and IsDelete=0", new SqlParameter { DbType = DbType.String, Value = id });
            RIRecordModel _result = null;
            try
            {
                _result = Mapper.Map<RIRecordModel>(lnRIRecord.SingleOrDefault(query));
            }
            catch (Exception ex)
            {

            }
            return _result;
        }

        public bool ExistsRecord(string DocID, string id)
        {
            var query = new Sql("select * from [APlatformAppSchema].[lnRIRecord] where DocID=@0 and IsDelete=0 and DocID <> @1",
                new SqlParameter { DbType = DbType.String, Value = DocID },
                new SqlParameter { DbType = DbType.String, Value = id });

            lnRIRecord record = null;
            try
            {
                record = lnRIRecord.SingleOrDefault(query);
                if (record != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public long GetMaxId()
        {
            long result = lnRIRecord.repo.ExecuteScalar<long>("SELECT max(convert(bigint,DocID)) FROM [APlatformAppSchema].[lnRIRecord] ");
            if (result <= 0)
            {
                return Convert.ToInt64(DateTime.Now.ToString("yyyyMMdd" + "01"));
            }
            else
            {
                return result + 1;
            }
        }


        public Page<lnRIRecord> GetRecordByKeyWordWithPage(Page<lnRIRecord> pageEntity, string keyWord)
        {
            var formatedKeyword = string.Format("%{0}%", keyWord);
            var query = new Sql();

            query.Append(@"select * from [APlatformAppSchema].[lnRIRecord] where IsDelete=0");

            if (!string.IsNullOrEmpty(keyWord))
            {
                query.Append(" and (DocID like @0 or Title like @0 or Author like @0 or Summary like @0 )", formatedKeyword);
            }
            query.Append(" ORDER BY Convert(bigint,[EntityID]) Desc,CREATETIME Desc ");

            Page<lnRIRecord> lnRIRecordPageEntity = null;
            try
            {
                lnRIRecordPageEntity = lnRIRecord.Page(pageEntity.CurrentPage, pageEntity.ItemsPerPage, query);
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordDataProvider>("Get lnRIRecord By KeyWord With Page Failed! \n", ex);
            }
            return lnRIRecordPageEntity;
        }

        public RIRecordModel AddRecord(RIRecordModel _et1, LnRIRecordEntityModel _et2)
        {
            RIRecordModel result = null;
            lnRIRecord _r1 = null;
            lnRIRecordEntity _r2 = null;


            _r1 = Mapper.Map<lnRIRecord>(_et1);
            _r2 = Mapper.Map<lnRIRecordEntity>(_et2);

            _r1.Insert();
            _r2.Insert();
            
            result = _et1;

            return result;
        }


        public bool ExistsRecordTitle(string _title)
        {
            var query = new Sql("select * from [APlatformAppSchema].[lnRIRecord] where Title=@0 and IsDelete=0 ",
                new SqlParameter { DbType = DbType.String, Value = _title });

            List<lnRIRecord>  record = new List<lnRIRecord>();
            try
            {
                record = lnRIRecord.Fetch(query);
                if (record.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取老版本record
        /// </summary>
        /// <param name="_et"></param>
        /// <returns></returns>
        public int GetOldVersionRecordMax(RIRecordModel _et)
        {

            int result = lnRIRecord.repo.ExecuteScalar<int>("SELECT max(Version) FROM [APlatformAppSchema].[lnRIRecord] WHERE IsDelete=0 and EntityID = @0 ",
                new SqlParameter { DbType = DbType.String, Value = _et.EntityID });


            return result;
        }

        public RIRecordModel UpdOldVersionRecord(RIRecordModel _et)
        {

            var query = new Sql("UPDATE [APlatformAppSchema].[lnRIRecord] SET IsLatest = 0,UPDATETIME = GETDATE() where IsDelete=0 and EntityID = @0 ",
                new SqlParameter { DbType = DbType.String, Value = _et.EntityID });

            lnRIRecord.repo.Execute(query);


            return _et;
        }


        public LnRIRecordEntityModel UpdRecordEntity(LnRIRecordEntityModel _et1)
        {

            lnRIRecordEntity _r1 = null;

            _r1 = Mapper.Map<lnRIRecordEntity>(_et1);


            var query = new Sql("UPDATE [APlatformAppSchema].[lnRIRecordEntity] SET LatestDocID = @0,UPDATETIME = GETDATE() where EntityID = @1 ",
                new SqlParameter { Value = _et1.LatestDocID },
                new SqlParameter { Value = _et1.EntityID });
            try
            {
                lnRIRecord.repo.Execute(query);

            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordDataProvider>("UPd recordentity Failed! \n", ex);

            }

            return _et1;
        }

        /// <summary>
        /// 当删除record时,包括删除最新版本的情况下 ,把当前entity的所有islatest=0，最新的那条由另外sql更新
        /// </summary>
        /// <param name="_eid"></param>
        public void UPDOldRecordIsLast(string _eid)
        {
            var query = new Sql(" UPDATE [APlatformAppSchema].[lnRIRecord] SET IsLatest = 0 WHERE EntityID = @0 AND IsDelete = 0 ",
                new SqlParameter { Value = _eid });
            try
            {
                lnRIRecord.repo.Execute(query);

            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordDataProvider>("UPd Record old islast Failed! \n", ex);
            }
            
        }

        /// <summary>
        /// 紧接着上面的,把当前isdelete=0的所有记录里最新的那条记录的islatest=1
        /// </summary>
        /// <param name="_eid"></param>
        public void UPDNewRecordIsLast(string _eid)
        {
            var query = new Sql(@" UPDATE [APlatformAppSchema].[lnRIRecord] SET IsLatest = 1,UPDATETIME=GETDATE() WHERE EntityID = @0 
AND DocID = (SELECT TOP 1 DocID FROM [APlatformAppSchema].[lnRIRecord] WHERE IsDelete = 0 AND EntityID = @1 ORDER BY [Version] DESC ) ",
                new SqlParameter { Value = _eid }, new SqlParameter { Value = _eid });
            try
            {
                lnRIRecord.repo.Execute(query);

            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordDataProvider>("UPd Record new islast Failed! \n", ex);
            }

        }

        /// <summary>
        /// 删除record时,更新entity表 
        /// </summary>
        /// <param name="_eid"></param>
        public void UPDNewRecordEntityIsLast(string _eid)
        {
            var query = new Sql(@" UPDATE [APlatformAppSchema].[lnRIRecordEntity] 
SET Updatetime= GETDATE(), LatestDocID = (SELECT TOP 1 DocID FROM [APlatformAppSchema].[lnRIRecord] WHERE IsDelete = 0 AND EntityID = @0 ORDER BY [Version] DESC)
 WHERE EntityID = @1 ",
                new SqlParameter { Value = _eid }, new SqlParameter { Value = _eid });
            try
            {
                lnRIRecord.repo.Execute(query);
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordDataProvider>("UPd Record entity Failed! \n", ex);
            }

        }

        /// <summary>
        /// 删掉checkitem
        /// </summary>
        /// <param name="_docid"></param>
        public void DelCheckItem(string _docid)
        {
            var query = new Sql(@" DELETE  FROM [APlatformAppSchema].[lnRICheckItem]
                                    WHERE   EntityID IN (
                                            SELECT DISTINCT
                                                    b.EntityID
                                            FROM    [APlatformAppSchema].[lnRIRelRecordCheckItem] AS a
                                                    INNER JOIN [APlatformAppSchema].[lnRICheckItem] AS b ON a.CheckItemID = b.DocID
                                            WHERE   a.DocID = @0 )  ",
                new SqlParameter { Value = _docid });
            try
            {
                lnRIRecord.repo.Execute(query);
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordDataProvider>("DelCheckItem  Failed! \n", ex);
            }

        }

        public void DelRefCheckItem(string _docid)
        {
            var query = new Sql(@" DELETE FROM  [APlatformAppSchema].[lnRIRelRecordCheckItem] WHERE DocID = @0  ",
                new SqlParameter { Value = _docid });
            try
            {
                lnRIRecord.repo.Execute(query);
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordDataProvider>("DelRefCheckItem Failed! \n", ex);
            }

        }


        /// <summary>
        /// 找出当前document关联的最新的checkitem
        /// </summary>
        /// <param name="_docid"></param>
        /// <returns></returns>
        public List<RICheckItemModel> GetCheckItemListReadytoRef(string _docid)
        {
            var query = new Sql(@" SELECT * FROM [APlatformAppSchema].[lnRICheckItem] with(nolock)
WHERE DocID IN (SELECT CheckItemID FROM [APlatformAppSchema].[lnRIRelRecordCheckItem] with(nolock) WHERE DocID = @0
)
AND IsLatest = 0  ", new SqlParameter { Value = _docid });

            List<RICheckItemModel> checkItem = Mapper.Map<List<RICheckItemModel>>(lnRICheckItem.Fetch(query));

            return checkItem;
        }

    }
}
