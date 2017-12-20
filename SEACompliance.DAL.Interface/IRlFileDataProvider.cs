using SEACompliance.DataBase;
using SEACompliance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.DAL.Interface
{
    public interface IRlFileDataProvider: IDALProvider
    {
        List<RIFileModel> GetFilesById(string documentId, string flagInfo = null);

        RIFileModel GetFileBydocId(string docId);

        void DeleteFileById(string docId);

        void DelFileByDocmentIdForZero(string _userName);

        RIFileModel UpdateFile(RIFileModel cplnRI);

        void UpdateFilesByDocumentId(List<string> _list, string documentId);

        RIFileModel CreateFileInfo(RIFileModel cplnRI, out string docId);

        int GetFileCountByFileName(string fileName);
    }
}
