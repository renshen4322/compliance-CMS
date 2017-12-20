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
using AutoMapper;
using SEACompliance.Model;

namespace SEACompliance.DAL
{
    public class lnRICheckItemDataProvider : IRICheckItemDataProvider
    {
        

        public RICheckItemModel AddCheckItem(RICheckItemModel cplnRICheckItem)
        {
            var sql = new Sql(@"INSERT INTO [APlatformAppSchema].[lnRICheckItem]
                                   ([DocID]
                                   ,[EntityID]
                                   ,[Version]
                                   ,[IsLatest]
                                   ,[CREATETIME]
                                   ,[UPDATETIME]
                                   ,[Path]
                                   ,[IsDelete]
                                   ,[MetaPath]
                                   ,[Title]
                                   ,[ReasonCodes]
                                   ,[Importance]
                                   ,[Risk]
                                   ,[EffectiveDate]
                                   ,[HasMeta]
                                   ,[CheckItemContent]
                                   ,[Penalty],[UnderReview])
                             VALUES
                                   (@0
                                   ,@1
                                   ,@2
                                   ,@3
                                   ,@4
                                   ,@5
                                   ,@6
                                   ,@7
                                   ,@8
                                   ,@9
                                   ,@10
                                   ,@11
                                   ,@12
                                   ,@13
                                   ,@14
                                   ,@15
                                   ,@16,@17)",
                 new SqlParameter { Value = cplnRICheckItem.DocID },
                 new SqlParameter { Value = cplnRICheckItem.EntityID },
                 new SqlParameter { Value = cplnRICheckItem.Version },
                 new SqlParameter { Value = cplnRICheckItem.IsLatest },
                 new SqlParameter { Value = cplnRICheckItem.CREATETIME },
                 new SqlParameter { Value = cplnRICheckItem.UPDATETIME },
                 new SqlParameter { Value = cplnRICheckItem.Path },
                 new SqlParameter { Value = cplnRICheckItem.IsDelete },
                 new SqlParameter { Value = cplnRICheckItem.MetaPath },
                 new SqlParameter { Value = cplnRICheckItem.Title },
                 new SqlParameter { Value = cplnRICheckItem.ReasonCodes },
                 new SqlParameter { Value = cplnRICheckItem.Importance },
                 new SqlParameter { Value = cplnRICheckItem.Risk },
                 new SqlParameter { Value = cplnRICheckItem.EffectiveDate },
                 new SqlParameter { Value = cplnRICheckItem.HasMeta },
                 new SqlParameter { Value = cplnRICheckItem.CheckItemContent },
                 new SqlParameter { Value = cplnRICheckItem.Penalty },
                 new SqlParameter { Value = cplnRICheckItem.UnderReview });


            RICheckItemModel result = null;
            try
            {
                var ItemId = lnRICheckItem.Fetch(sql);
                result = Mapper.Map<RICheckItemModel>(cplnRICheckItem);
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRICheckItemDataProvider>("Add lnRICheckItem Failed! \n", ex);
            }
            return result;
        }

        public RICheckItemModel UpdateCheckItem(RICheckItemModel cplnRICheckItem)
        {
            lnRICheckItem result = null;
            try
            {
                result = Mapper.Map<lnRICheckItem>(cplnRICheckItem);
                result.Update();

            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRICheckItemDataProvider>("Update RICheckitem Failed! \n", ex);
            }
            cplnRICheckItem = Mapper.Map<RICheckItemModel>(result);
            return cplnRICheckItem;
        }

        public RICheckItemModel GetById(string id)
        {
            var query = new Sql("select * from [APlatformAppSchema].[lnRICheckItem] where DocID=@0 and IsDelete=0", new SqlParameter { DbType = DbType.String, Value = id });
            RICheckItemModel checkItem = null;
            try
            {
                checkItem = Mapper.Map<RICheckItemModel>(lnRICheckItem.SingleOrDefault(query));
            }
            catch (Exception ex)
            {

            }
            return checkItem;
        }

        public bool ExistsCheckItem(string DocID, string id)
        {
            var query = new Sql("select * from [APlatformAppSchema].[lnRICheckItem] where DocID=@0 and IsDelete=0 and DocID <> @1",
                new SqlParameter { DbType = DbType.String, Value = DocID },
                new SqlParameter { DbType = DbType.String, Value = id });

            lnRICheckItem item = null;
            try
            {
                item = lnRICheckItem.SingleOrDefault(query);
                if (item != null)
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

        public List<RICheckItemModel> GetCheckItemList(string id)
        {
            var query = new Sql(@" SELECT  d.*
        FROM    [APlatformAppSchema].[lnRIRecord] a
                RIGHT JOIN [APlatformAppSchema].[lnRIRelRecordCheckItem] b ON a.DocID = b.DocID
                RIGHT JOIN [APlatformAppSchema].[lnRICheckItem] c ON c.DocID = b.CheckItemID
				 RIGHT JOIN [APlatformAppSchema].[lnRICheckItem] d ON d.EntityID = c.EntityID AND c.Version >= d.Version
				  RIGHT JOIN [APlatformAppSchema].[lnRICheckItem] f ON f.EntityID = c.EntityID AND f.Version =1
        WHERE   a.IsDelete = 0
                AND a.DocID = @0
				ORDER BY f.CREATETIME ",
                                  new SqlParameter { DbType = DbType.String, Value = id });

            List<RICheckItemModel> checkItem = null;
            try
            {
                checkItem = Mapper.Map<List<RICheckItemModel>>(lnRICheckItem.Fetch(query));
            }
            catch (Exception ex)
            {

            }
            return checkItem;
        }

        public bool DeleteCheckItemById(string id)
        {
            var query = new Sql("delete from [APlatformAppSchema].[lnRICheckItem] where DocID=@0",
                new SqlParameter { DbType = DbType.String, Value = id });
            try
            {
                var ItemId = lnRICheckItem.repo.Execute(query);
                return ItemId > 0;
            }
            catch
            {
                return false;
            }
        }

        public long GetMaxId()
        {
            long result = lnRICheckItem.repo.ExecuteScalar<long>("SELECT max(convert(bigint,DocID)) FROM [APlatformAppSchema].[lnRICheckItem]");
            if (result <= 0)
            {
                return Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHH"));
            }
            else
            {
                return result + 1;
            }
        }

        public bool DeleteCheckItemById2(string _docid)
        {
            var query = new Sql("delete from [APlatformAppSchema].[lnRICheckItem] where DocID=@0",
                new SqlParameter { DbType = DbType.String, Value = _docid });
            try
            {
                var ItemId = lnRICheckItem.repo.Execute(query);
                return ItemId > 0;
            }
            catch
            {
                return false;
            }
        }


        public int GetMaxVersion(string _entityid)
        {
            int result = 1;
            result = lnRICheckItem.repo.ExecuteScalar<int>("SELECT max(convert(int,Version)) FROM [APlatformAppSchema].[lnRICheckItem] WHERE EntityID = @0 and IsDelete = 0 "
                , new SqlParameter { DbType = DbType.String, Value = _entityid });
            return result;
        }


        public bool UpdateCheckitemOldVersionLatest(string _eid)
        {
            var query = new Sql(" UPDATE [APlatformAppSchema].[lnRICheckItem]  SET IsLatest = 0 WHERE EntityID = @0 AND IsDelete = 0 ",
                new SqlParameter { DbType = DbType.String, Value = _eid });
            try
            {
                var ItemId = lnRICheckItem.repo.Execute(query);
                return ItemId > 0;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateCheckItem2(RICheckItemModel _et)
        {
            var query = new Sql(@" UPDATE [APlatformAppSchema].[lnRICheckItem]  SET 
       Version = @0,
       IsLatest = @1,
       UPDATETIME = GETDATE(),
        Penalty = @2,
       Path = @3,
       IsDelete = @4,
       MetaPath = @5,
       CheckItemContent = @6,
       ReasonCodes = @7,
       Importance = @8,
       Risk = @9,
       HasMeta = @10,
        UnderReview = @13
        WHERE DocID = @11 AND EntityID = @12 ",
                new SqlParameter { Value = _et.Version }
                , new SqlParameter { Value = _et.IsLatest }
                , new SqlParameter { Value = _et.Penalty }
                , new SqlParameter { Value = _et.Path }
                , new SqlParameter { Value = _et.IsDelete }
                , new SqlParameter { Value = _et.MetaPath }
                , new SqlParameter { Value = _et.CheckItemContent }
                , new SqlParameter { Value = _et.ReasonCodes }
                , new SqlParameter { Value = _et.Importance }
                , new SqlParameter { Value = _et.Risk }
                , new SqlParameter { Value = _et.HasMeta }
                , new SqlParameter { Value = _et.DocID }
                , new SqlParameter { Value = _et.EntityID }
                , new SqlParameter { Value = _et.UnderReview }
                );

            try
            {
                lnRICheckItem.repo.Execute(query);
            }
            catch (Exception ex)
            {

            }

        }


        public bool UpdateCheckitemOldUnderReview(string _eid)
        {
            var query = new Sql(" UPDATE [APlatformAppSchema].[lnRICheckItem]  SET UnderReview = 0 WHERE EntityID = @0 AND IsDelete = 0 ",
                new SqlParameter { DbType = DbType.String, Value = _eid });
            try
            {
                var ItemId = lnRICheckItem.repo.Execute(query);
                return ItemId > 0;
            }
            catch
            {
                return false;
            }
        }


    }
}
