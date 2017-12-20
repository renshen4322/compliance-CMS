using SEACompliance.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.Model;
using SEACompliance.DataBase;
using AutoMapper;
using Umbraco.Core.Persistence;
using Umbraco.Core.Logging;
using SEACompliance.Core.ExceptionApi;

namespace SEACompliance.DAL
{
    public class RIFileContentDataProvider : IRIFileContentDataProvider
    {
        public List<RIFileContentModel> GetFileExtension()
        {
            var query = new Sql("select * from [APlatformAppSchema].[lnRIFileContent] ");
            List<RIFileContentModel> list = null;
            try
            {
                List<lnRIFileContent> riList = lnRIFileContent.Fetch(query);
                list = Mapper.Map<List<RIFileContentModel>>(riList);
            }
            catch (Exception ex)
            {
                LogHelper.Error<RlFileDataProvider>("Get All lnRIFileContent Failed! \n", ex);
                throw new RequestErrorException(string.Format("Get All lnRIFileContent Failed_{0}_!",  "__" + ex.Message));
            }

            return list;
        }
    }
}
