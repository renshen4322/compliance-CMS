using SEACompliance.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEACompliance.Model;
using SEACompliance.DAL.Interface;
using SEACompliance.Core.ExceptionApi;
using System.IO;
using System.Configuration;
using SEACompliance.Core.Common;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace SEACompliance.Service
{
    public class RlFileService : IRlFileService
    {
        private IRlFileDataProvider _iRlFileDataProvider;
        public RlFileService(IRlFileDataProvider iRlFileDataProvider)
        {
            _iRlFileDataProvider = iRlFileDataProvider;
        }
      

        public List<RIFileModel> GetFilesById(string documentId, string flagInfo = null)
        {
            if (string.IsNullOrEmpty(documentId))
            {
                throw new RequestErrorException("documentId is null");
            }          
            return _iRlFileDataProvider.GetFilesById(documentId, flagInfo);
        }

        public RIFileModel GetFileBydocId(string docId)
        {
            if (string.IsNullOrEmpty(docId))
            {
                throw new RequestErrorException("docId is null");
            }
            return _iRlFileDataProvider.GetFileBydocId(docId);
        }

        public void DeleteFileById(string docId)
        {
            _iRlFileDataProvider.DeleteFileById(docId);
        }

        public void DelFileByDocmentIdForZero(string _userName)
        {
            _iRlFileDataProvider.DelFileByDocmentIdForZero(_userName);
        }

        public RIFileModel UpdateFile(RIFileModel cplnRI)
        {
            if (string.IsNullOrEmpty(cplnRI.Title))
            {
                throw new RequestErrorException("RlFile title is null");
            }
            return _iRlFileDataProvider.UpdateFile(cplnRI);
        }

        public void UpdateFilesByDocumentId(List<string> _list, string documentId)
        {
             _iRlFileDataProvider.UpdateFilesByDocumentId(_list,documentId);
        }
        public RIFileModel CreateFileInfo(RIFileModel cplnRI, out string docId)
        {
            if (string.IsNullOrEmpty(cplnRI.Path))
            {
                throw new RequestErrorException("RlFile File is null");
            }
            if (string.IsNullOrEmpty(cplnRI.Title))
            {
                throw new RequestErrorException("RlFile title cannot be empty");
            }
            if (string.IsNullOrEmpty(cplnRI.Content))
            {
                cplnRI.Content = "";
            }
            if (string.IsNullOrEmpty(cplnRI.DocumentID))
            {
                throw new RequestErrorException("RlFile DocumentID is null");
            }
            return _iRlFileDataProvider.CreateFileInfo(cplnRI, out docId);
        }

        public RIFileModel CreateFileByUrl(RIFileModel cplnRI, out string docId)
        {
            if (string.IsNullOrEmpty(cplnRI.Path))
            {
                throw new RequestErrorException("RlFile File is null");
            }
            if (!cplnRI.Path.StartsWith("http"))
            {
                throw new RequestErrorException("Please enter the correct url and Starting with HTTP");
            }
            else
            {
                cplnRI.MimeType = "url";
                cplnRI.DocID = System.Guid.NewGuid().ToString("N");
                cplnRI.FileName = "";
            }
            if (string.IsNullOrEmpty(cplnRI.Title))
            {
                //cplnRI.Title = "";
                throw new RequestErrorException("RlFile title cannot be empty");
            }
            if (string.IsNullOrEmpty(cplnRI.Content))
            {
                cplnRI.Content = "";
            }
            if (string.IsNullOrEmpty(cplnRI.DocumentID))
            {
                throw new RequestErrorException("RlFile DocumentID is null");
            }
            return _iRlFileDataProvider.CreateFileInfo(cplnRI, out docId);
        }

        public int GetFileCountByFileName(string fileName)
        {
            return _iRlFileDataProvider.GetFileCountByFileName(fileName);
        }

        public string GetDirsAndFileName(string fileName, string documentId, string appString)
        {
            string fullName = "";
            string pathName = "";
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            try
            {
                string name = Guid.NewGuid().ToString() + GetExtensions(GetRealFileName(fileName));
                string toolDircs = ConfigurationManager.AppSettings[appString].ToString();        
                pathName = Path.Combine(year, month, day, documentId).Replace("/", "\\");
                string pa2 = Path.Combine(toolDircs, pathName);
                CreateDircs(pa2);
                fullName = Path.Combine("/", pathName, name).Replace('\\', '/');
            }
            catch (Exception ex)
            {
                throw new RequestErrorException("Create fullName Failed"+ex.Message);
            }
            return fullName;
        }

        public string GetDirsForCopyAndFileName( string appString,string oldPath)
        {
            string fullName = "";
            string pathName = "";
            string copy = "copy";           
            try
            {
                var _newPath = oldPath.Substring(oldPath.LastIndexOf("/")+1);
                string name = _newPath;
                string toolDircs = ConfigurationManager.AppSettings[appString].ToString();
                pathName = Path.Combine(copy).Replace("/", "\\");
                string pa2 = Path.Combine(toolDircs, pathName);
                CreateDircs(pa2);
                fullName = Path.Combine("/", pathName, name).Replace('\\', '/');
            }
            catch (Exception ex)
            {
                throw new RequestErrorException("Create fullName Failed");
            }
            return fullName;
        }

        public void CreateDircs(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        public string GetRealFileName(string fileName)
        {
            if (fileName.Contains("\\"))
            {
                string[] name = fileName.Split('\\');
                return name[name.Length - 1];
            }
            else
            {
                string[] name = fileName.Split('/');
                return name[name.Length - 1];
            }
        }

        public string GetExtensions(string fileName)
        {
            if (fileName.Contains("."))
            {
                return fileName.Substring(fileName.LastIndexOf("."));
            }
            return fileName;
        }

        public void DeleteFileDiec(string path,string appString)
        {
            string url = "";
            try
            {
                string toolDircs = ConfigurationManager.AppSettings[appString].ToString();
                url = Path.Combine(toolDircs + path);
                if (File.Exists(url))
                {
                    File.Delete(url);
                }

            }
            catch (Exception ex)
            {
                throw new RequestErrorException("delete file failed path to " + path);
            }
        }

        public string GetDownLoadUrl(string path, string documentId)
        {
            string url = "";
            try
            {
                if (!string.IsNullOrEmpty(path))
                    path.Replace('/', '\\');
                if (!string.IsNullOrEmpty(documentId))
                {
                    string toolDircs = ConfigurationManager.AppSettings["Aplatform.CMSContentDircs"].ToString();
                    url = Path.Combine(toolDircs + path);                    
                }
                else
                {
                    string toolDircs = ConfigurationManager.AppSettings["Aplatform.CMSDircs"].ToString();
                    url = Path.Combine(toolDircs + path);
                }
            }
            catch (Exception ex)
            {

                throw new RequestErrorException("LoadUrl path error");
            }

            return url;
        }
        public RIFileModel GetModelStream(RIFileModel model)
        {
            using (FileStream fs = File.Open(model.Path, FileMode.Open, FileAccess.Read))
            {
                model.Stream = new MemoryStream();
                byte[] bts = new byte[fs.Length];
                fs.Read(bts, 0, bts.Length);
                model.Stream.Write(bts, 0, bts.Length);
            }
            return model;
        }
       
        public string GetDownLoadFileName(string fileName, string path)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }
            return GetRealFileName(path.Replace('/', '\\'));
        }

        public void GetCopyOldFileToNewPath(string fromPath,string targetPath)
        {
            if (File.Exists(fromPath))
            {
                File.Copy(fromPath, targetPath,true);                
            }
          
        }

        public bool checkThisPathIsExist(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            return false;
        }

        public void DowloadInfo(RIFileModel model,string path)
        {
            model.Path = path;
            model = GetModelStream(model);
            var downLoadName = GetDownLoadFileName(model.FileName, model.Path);
            var filename = "";
            if (HttpContext.Current.Request.Browser.Browser == "IE" ||
            Regex.IsMatch(HttpContext.Current.Request.UserAgent, @"Trident/7.*rv:11"))
                filename = HttpUtility.UrlEncode(downLoadName);
            filename = ContentDispositionUtil.GetHeaderValue(downLoadName);
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            HttpContext.Current.Response.ContentType = model.MimeType;
            HttpContext.Current.Response.AddHeader("Content-disposition", filename);
            HttpContext.Current.Response.BinaryWrite(model.Stream.GetBuffer());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}
