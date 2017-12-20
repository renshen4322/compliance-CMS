using SEACompliance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.DAL.Interface
{
    public interface IRIFileContentDataProvider: IDALProvider
    {
        List<RIFileContentModel> GetFileExtension();
    }
}
