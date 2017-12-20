using SEACompliance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Service.Interface
{
    public interface IRIFileContentService: IBLLService
    {
        List<RIFileContentModel> GetFileExtension();
    }
}
