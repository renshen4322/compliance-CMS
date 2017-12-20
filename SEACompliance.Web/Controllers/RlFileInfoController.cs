using AutoMapper;
using log4net;
using SEACompliance.Core.Autofac;
using SEACompliance.Core.Common;
using SEACompliance.Core.Json;
using SEACompliance.Model;
using SEACompliance.Service.Interface;
using SEACompliance.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Core.Security;
using Umbraco.Core;


namespace SEACompliance.Web.Controllers
{
    public class RlFileInfoController : UmbracoAuthorizedController
    {
        private IRlFileService _IRlFileService;
        private IRIFileContentService _IRIFileContentService;
        ILog _log = log4net.LogManager.GetLogger("RlFileInfoController_Info");
        public RlFileInfoController(IRlFileService rlFileService, IRIFileContentService rIFileContentService)
        {
            _IRlFileService = rlFileService;//ContainerManager.Resolve<IRlFileService>();
            _IRIFileContentService = rIFileContentService;//ContainerManager.Resolve<IRIFileContentService>();
        }

        [HttpPost]
        [Route("~/umbraco/backoffice/RlFileInfo/UploadFile", Name = "UploadFileInfo ")]
        public ActionResult UploadFile(RIFileDTOModel fm)
        {
            var result = new JsonResultModel<RIFileDTOModel> { Status = JsonResponseStatus.Failed };
            var listExtension = _IRIFileContentService.GetFileExtension();
            string docId = "";
            var auth = HttpContext.User.Identity;
            var backofficeUser = "";
            if (auth != null)
            {
                backofficeUser = auth.Name;
            }
            if (Request.Files["file"] != null && !string.IsNullOrEmpty(Request.Files["file"].FileName))
            {
                var file = Request.Files["file"];
                string realName = _IRlFileService.GetRealFileName(file.FileName);
                var fileExtension = realName.Split('.');
                RIFileModel _fileModel = null;
                if (listExtension.Any(o => o.FileExtension.IndexOf(fileExtension[fileExtension.Length - 1], StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    try
                    {
                        int i = file.FileName.LastIndexOf(".");
                        string _subStr = file.FileName.Substring(i).ToLower();
                        string _cotType = "";
                        foreach (var item in listExtension)
                        {
                            if (item.FileExtension.Contains("|"))
                            {
                                var _filExt = item.FileExtension.Split('|');
                                for (int j = 0; j < _filExt.Length; j++)
                                {
                                    if (_filExt[j].Equals(_subStr))
                                    {
                                        _cotType = item.Content.ToLower();
                                    }
                                }

                            }
                            else if (item.FileExtension.Equals(_subStr))
                            {
                                _cotType = item.Content.ToLower();
                            }
                        }
                        string severFileName = _IRlFileService.GetDirsAndFileName(file.FileName, fm.DocumentID, "Aplatform.CMSContentDircs");
                        RIFileDTOModel model = new RIFileDTOModel();
                        model.DocID = System.Guid.NewGuid().ToString("N");
                        model.Title = fm.Title;
                        model.Content = fm.Content;
                        model.MimeType = "application/" + _cotType;
                        model.Path = severFileName;
                        model.FileName = realName;
                        model.DocumentID = fm.DocumentID;
                        model.CreateUser = backofficeUser;
                        _fileModel = Mapper.Map<RIFileModel>(model);
                        var _createFile = _IRlFileService.CreateFileInfo(_fileModel, out docId);
                        var path = this._IRlFileService.GetDownLoadUrl(_createFile.Path, _createFile.DocumentID);
                        file.SaveAs(path);
                        result.Data = Mapper.Map<RIFileDTOModel>(_createFile);
                        result.Status = JsonResponseStatus.Success;
                        result.Code = "upload file success";
                    }
                    catch (Exception ex)
                    {
                        _IRlFileService.DeleteFileById(docId);
                        result.Status = JsonResponseStatus.Failed;
                        result.Code = ex.Message;
                    }
                }
            }
            return new JsonResult<JsonResultModel<RIFileDTOModel>>(result);
        }

        [HttpGet]
        [Route("~/umbraco/backoffice/RlFileInfo/DownLoadFile", Name = "DownLoadFileInfo ")]
        public ActionResult DownLoadFile(string docId)
        {
            var result = new JsonResultModel<RIFileDTOModel> { Status = JsonResponseStatus.Failed };
            try
            {
                var model = this._IRlFileService.GetFileBydocId(docId);
                var path = this._IRlFileService.GetDownLoadUrl(model.Path, model.DocumentID);
                if (_IRlFileService.checkThisPathIsExist(path))
                {
                    _IRlFileService.DowloadInfo(model, path);
                    result.Status = JsonResponseStatus.Success;
                }
                else
                {
                    string _newServerPath = _IRlFileService.GetDirsForCopyAndFileName("Aplatform.CMSDircs", path);
                    string tagerDircs = ConfigurationManager.AppSettings["Aplatform.CMSDircs"].ToString();
                    string _targetPath = Path.Combine(tagerDircs + _newServerPath);
                    model.Path = _targetPath;
                    _IRlFileService.DowloadInfo(model, _targetPath);
                    result.Status = JsonResponseStatus.Success;
                }

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Failed;
                result.Code = ex.Message;
               // _log.ErrorFormat("Failed to DownLoadFile, docId {0},ErrorMessage={1}", docId, ex.Message);
                return new JsonResult<JsonResultModel<RIFileDTOModel>>(result);
            }
        }

    }
}