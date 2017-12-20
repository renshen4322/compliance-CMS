using SEACompliance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Service.Interface
{
    public interface IRlFileService: IBLLService
    {
        List<RIFileModel> GetFilesById(string documentId, string flagInfo = null);

        RIFileModel GetFileBydocId(string docId);

        void DeleteFileById(string docId);

        void DelFileByDocmentIdForZero(string _userName);

        RIFileModel UpdateFile(RIFileModel cplnRI);

        void UpdateFilesByDocumentId(List<string> _list, string documentId);

        RIFileModel CreateFileInfo(RIFileModel cplnRI, out string docId);

        RIFileModel CreateFileByUrl(RIFileModel cplnRI, out string docId);

        int GetFileCountByFileName(string fileName);

        string GetDirsAndFileName(string fileName, string documentId, string appString);

        void CreateDircs(string path);

        string GetRealFileName(string fileName);

        string GetExtensions(string fileName);

        void DeleteFileDiec(string path, string appString);

        string GetDownLoadUrl(string path, string documentId);

        RIFileModel GetModelStream(RIFileModel model);

        string GetDownLoadFileName(string fileName, string path);

        void GetCopyOldFileToNewPath(string fromPath, string targetPath);

        string GetDirsForCopyAndFileName(string appString, string oldPath);

        bool checkThisPathIsExist(string path);

        void DowloadInfo(RIFileModel model, string path);
    }
}
