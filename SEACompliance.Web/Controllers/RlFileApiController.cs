using AutoMapper;
using SEACompliance.Model;
using SEACompliance.Service.Interface;
using SEACompliance.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SEACompliance.Core.Json;
using SEACompliance.Core.Extensions;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using SEACompliance.Core.ExceptionApi;
using log4net;
using SEACompliance.Core.Autofac;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using SEACompliance.Core.Common;
using System.Text.RegularExpressions;
using System.Configuration;
using SEACompliance.Core.Web;
using Umbraco.Core.Security;
using Umbraco.Core;

namespace SEACompliance.Web.Controllers
{
    [PluginController("SEACompliance")]
    public class RlFileApiController : UmbracoAuthorizedJsonController
    {
        private IRlFileService _IRlFileService;
        
        ILog _log = log4net.LogManager.GetLogger("RlFileApiController_Info");
        
        public RlFileApiController(IRlFileService rlFileService)
        {
            _IRlFileService = rlFileService;           
           
        }

        [HttpGet]
        public JsonResultModel<List<RIFileDTOModel>> GetAllFiles(string documentId)
        {
            var result = new JsonResultModel<List<RIFileDTOModel>> { Status = JsonResponseStatus.Success };
            try
            {
                var _createUser = new HttpContextWrapper(HttpContext.Current).GetUmbracoAuthTicket().Name;
                var record = _IRlFileService.GetFilesById(documentId, _createUser);
                if (record != null && record.Count > 0)
                {
                    var riFileViewModel = Mapper.Map<List<RIFileDTOModel>>(record);
                    result.Status = JsonResponseStatus.Success;
                    result.Data = riFileViewModel;
                }
            }
            catch (Exception ex)
            {
                result.Code = ex.Message;
                result.Status = JsonResponseStatus.Failed;
            }
            return result;
        }

        [HttpPost]
        public JsonResultModel<RIFileDTOModel> DeleteFileInfo(string docId)
        {
            var result = new JsonResultModel<RIFileDTOModel> { Status = JsonResponseStatus.Failed };
            try
            {
                ////var a = 0;
                ////var test = 10 / a;

                var mFile = _IRlFileService.GetFileBydocId(docId);
                var _modelFile = _IRlFileService.GetFileBydocId(docId);
                if (mFile == null)
                {
                    throw new RequestErrorException("File Is Not Exist");
                }
                if (mFile.MimeType.Equals("url"))
                {
                    _IRlFileService.DeleteFileById(docId);
                    result.Status = JsonResponseStatus.Success;
                    result.Code = string.Format("delete {0} tableinfo success", docId);
                }
                else
                {
                    int _fileCount = _IRlFileService.GetFileCountByFileName(_modelFile.FileName);
                    string _newServerPath = _IRlFileService.GetDirsForCopyAndFileName("Aplatform.CMSDircs", _modelFile.Path);
                    string toolDircs = ConfigurationManager.AppSettings["Aplatform.CMSContentDircs"].ToString();
                    string tagerDircs = ConfigurationManager.AppSettings["Aplatform.CMSDircs"].ToString();
                    string _fromPath = Path.Combine(toolDircs + _modelFile.Path);
                    string _targetPath = Path.Combine(tagerDircs + _newServerPath);
                    if (_fileCount > 1)
                    {                      
                        _IRlFileService.GetCopyOldFileToNewPath(_fromPath, _targetPath);
                    }
                    else
                    {
                        _IRlFileService.DeleteFileDiec(_newServerPath, "Aplatform.CMSDircs");
                    }
                    _IRlFileService.DeleteFileDiec(mFile.Path, "Aplatform.CMSContentDircs");
                    _IRlFileService.DeleteFileById(docId);
                    result.Status = JsonResponseStatus.Success;
                    result.Code = string.Format("delete  {0} tableinfo and file success", docId);
                }

            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Error;
                result.Code = ex.Message;
                //_log.Error(string.Format("delete {0} file Failed:docid{1} " + ex.Message, docId));
            }
            return result;
        }

        [HttpPost]
        public JsonResultModel<RIFileDTOModel> ModifyFileInfo(RIFileDTOModel model)
        {
            var result = new JsonResultModel<RIFileDTOModel> { Status = JsonResponseStatus.Failed };
            RIFileModel _rIFileModel = null;
            try
            {
                _rIFileModel = Mapper.Map<RIFileModel>(model);
                var _fileInfo = _IRlFileService.UpdateFile(_rIFileModel);
                result.Status = JsonResponseStatus.Success;
                result.Data = Mapper.Map<RIFileDTOModel>(_fileInfo);
            }
            catch (Exception ex)
            {
                result.Code = ex.Message;
                result.Status = JsonResponseStatus.Failed;
            }
            return result;
        }

        [HttpPost]
        public JsonResultModel<RIFileDTOModel> UploadFileByUrl(RIFileDTOModel fm)
        {
            var result = new JsonResultModel<RIFileDTOModel> { Status = JsonResponseStatus.Failed };
            string docId = "";
            var auth = new HttpContextWrapper(HttpContext.Current).GetUmbracoAuthTicket();
            var backofficeUser = "";
            if (auth != null)
            {
                backofficeUser = auth.Name;
            }
            fm.CreateUser = backofficeUser;
            RIFileModel _fileMo = Mapper.Map<RIFileModel>(fm);
            try
            {
                var _infoFile = _IRlFileService.CreateFileByUrl(_fileMo, out docId);
                _fileMo.DocID = docId;
                result.Data = Mapper.Map<RIFileDTOModel>(_fileMo);
                result.Status = JsonResponseStatus.Success;
                result.Code = "save url success";
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Failed;
                result.Code = ex.Message;
            }
            return result;
        }


    }
}
