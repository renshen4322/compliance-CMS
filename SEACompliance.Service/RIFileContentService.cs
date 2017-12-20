using SEACompliance.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.Model;
using SEACompliance.DAL.Interface;
using AutoMapper;

namespace SEACompliance.Service
{
    public class RIFileContentService : IRIFileContentService
    {
        private IRIFileContentDataProvider _IRIFileContentDataProvider;
        public RIFileContentService(IRIFileContentDataProvider iRIFileContentDataProvider)
        {
            _IRIFileContentDataProvider = iRIFileContentDataProvider;
        }
       
        public List<RIFileContentModel> GetFileExtension()
        {
            return _IRIFileContentDataProvider.GetFileExtension();
        }
    }
}
