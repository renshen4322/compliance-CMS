using SEACompliance.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.Model;
using Umbraco.Core.Persistence;
using System.Data.SqlClient;
using SEACompliance.DataBase;
using AutoMapper;
using Umbraco.Core.Logging;
using System.Data;

namespace SEACompliance.DAL
{
    public class RlRelRecordToolsDataProvider : IRlRelRecordToolsProvider
    {
        public List<RIRelRecordToolModel> GetListToolsBydocId(string docId)
        {
            var query = new Sql("select * from [APlatformAppSchema].[lnRIRelRecordTools] where DocID=@0 ", new SqlParameter { DbType = DbType.String, Value = docId });
            List<RIRelRecordToolModel> list = null;
            try
            {
                List<lnRIRelRecordTool> riList = lnRIRelRecordTool.Fetch(query);
                list = Mapper.Map<List<RIRelRecordToolModel>>(riList);
            }
            catch (Exception ex)
            {
                LogHelper.Error<RlFileDataProvider>(string.Format("Get All lnRIRelRecordTools Failed by docid{0}! \n",docId), ex);
            }
            return list;
        }
    }
}
