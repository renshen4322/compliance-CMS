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

namespace SEACompliance.DAL
{
    public class RIRecordTypeProvider : IRIRecordTypeProvider
    {
        public List<RIRecord_Type> GetAll()
        {
            LogHelper.Info<RIRecordTypeProvider>("Get All RIRecordType hyx ");

            //var query = new Sql(" SELECT * FROM RIRecord_Type where Status = 0 ");
            var query = new Sql(" SELECT PackageID,Name,Abbreviation,Summary,ParentPackageID,Sequence,Category FROM APlatformAppSchema.lnComplianceModule WHERE IsDelete = 0 AND LEVEL = 0 ORDER BY PackageID ");
            List<RIRecord_Type> list = new List<RIRecord_Type>();
            List<lnComplianceModule> listtmp = null;
            try
            {
                listtmp = lnComplianceModule.Fetch(query);
                //list = RIRecord_Type.Fetch(query);
                foreach (var r in listtmp)
                {
                    int _status = 0;
                    int _depth = 0;
                    if (r.Category == "Module")
                    {
                        _depth = 1;
                    }
                    else if (r.Category == "Topic")
                    {
                        _depth = 2;
                    }
                    else if (r.Category == "Sub topic")
                    {
                        _depth = 3;
                    }
                    list.Add(new RIRecord_Type() { Code = r.PackageID, Category = r.Category, Label = r.Name, LabelDes = r.Summary, ParentCode = r.ParentPackageID, Status = _status, Depth = _depth});
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error<RIRecordTypeProvider>("Get All RIRecordType hyx Failed! \n", ex);
            }

            return list;
        }


        public RIRecord_Type AddRecordType(RIRecord_Type et)
        {
            RIRecord_Type result = null;
            try
            {
                et.AddTime = DateTime.Now;
                var recordId = Convert.ToInt64(et.Insert());
                result = et;
            }
            catch (Exception ex)
            {
                LogHelper.Error<RIRecordTypeProvider>("Add RIRecordType Failed! \n", ex);
            }
            return result;
        }

        public RIRecord_Type UpdateRecordType(RIRecord_Type et)
        {
            RIRecord_Type result = null;
            try
            {
                et.AddTime = DateTime.Now;
                et.Update();
                result = et;
            }
            catch (Exception ex)
            {
                LogHelper.Error<RIRecordTypeProvider>("Update RIRecordType Failed! \n", ex);
            }
            return result;
        }
        public bool DeleteRecordType(int id)
        {
            var query = new Sql("DELETE from RIRecord_Type WHERE Id=@0",
                new SqlParameter { DbType = DbType.String, Value = id });
            try
            {
                var ItemId = RIRecord_Type.repo.Execute(query);
                return ItemId > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
