using log4net;
using SEACompliance.Core.Autofac;
using SEACompliance.Core.ExceptionApi;
using SEACompliance.DAL.Interface;
using SEACompliance.DataBase;
using SEACompliance.Model;
using SEACompliance.Model.RIRecordXMLModel;
using SEACompliance.Service.Interface;
using System;
using System.Collections.Generic;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Web;
using System.Linq;
using SEACompliance.Core.Common;
using System.Threading;
using System.Web;

namespace SEACompliance.Service
{
    public class lnRIRecordService : IlnRIRecordService
    {
        private IlnRIRecordDataProvider _lnRIRecordDataProvider;
        private IMapperService _mapperService;
        private IlnRICheckItemService _checkItemService = ContainerManager.Resolve<IlnRICheckItemService>();
        private IlnRIRelRecordCheckItemService _recordCheckItemService = ContainerManager.Resolve<IlnRIRelRecordCheckItemService>();
        private IRlFileService _fileService = ContainerManager.Resolve<IRlFileService>();
        private static string _pa = System.Configuration.ConfigurationManager.AppSettings["XMLPath"].ToString();
        private string _path = "/" + _pa + "/";
        private string _xmlDBSavePath = System.Configuration.ConfigurationManager.AppSettings["XMLUrlPath"].ToString();
        ILog log = log4net.LogManager.GetLogger("RIRecordApiController_Info");
        string u = UmbracoContext.Current.Security.CurrentUser != null ? UmbracoContext.Current.Security.CurrentUser.Username.ToString() : string.Empty;
        private IRlFileDataProvider _iRlFileDataProvider;
        public lnRIRecordService(IlnRIRecordDataProvider lnRIRecordDataProvider, IMapperService mapperService, IRlFileDataProvider iRlFileDataProvider)
        {
            _lnRIRecordDataProvider = lnRIRecordDataProvider;
            _mapperService = mapperService;
            _iRlFileDataProvider = iRlFileDataProvider;
        }

        public lnRIRecordService()
        {
        }
        
        public RIRecordModel AddRecord(RIRecordModel cplnRIRecord)
        {
            return _lnRIRecordDataProvider.AddRecord(cplnRIRecord);
        }

        public RIRecordModel AddRecord(RIRecordModel _et1, LnRIRecordEntityModel _et2)
        {
            return _lnRIRecordDataProvider.AddRecord(_et1, _et2);
        }


        public RIRecordModel GetById(string id)
        {
            return _lnRIRecordDataProvider.GetById(id);
        }

        public bool ExistsRecord(string id, string DocID)
        {
            return _lnRIRecordDataProvider.ExistsRecord(id, DocID);
        }

        public long GetMaxId()
        {
            return _lnRIRecordDataProvider.GetMaxId();
        }
        public PageModel<RIRecordModel> GetRecordByKeyWordWithPage(PageModel<RIRecordModel> pageModel, string keyWord)
        {
            var pageEntity = _mapperService.MapModel<Page<lnRIRecord>>(pageModel);
            var returnPageEntity = _lnRIRecordDataProvider.GetRecordByKeyWordWithPage(pageEntity, keyWord);
            if (returnPageEntity != null)
            {
                return _mapperService.MapModel<PageModel<RIRecordModel>>(returnPageEntity);
            }
            return null;
        }


        public void ToXml(RIRecordModel record)
        {
            try
            {
                //Record转xml描述文件
                WrapXml obj = new WrapXml();
                obj.filename = record.EntityID + "_" + record.Version;
                obj.filepath = "crcd";
                obj._head = new head()
                {
                    documentID = record.DocID,
                    contentFormat = "doc",
                    contentType = "JP3_RI",
                    documentType = "crcd",
                    effectiveDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    elementID = "1",
                    entityID = record.EntityID,
                    expiredDate = "9999-12-31"
                    ,
                    subContentFormat = "snapshotFullDocument"
                };

                List<paragraph> _p = new List<paragraph>();
                foreach (var item in record.CheckItem)
                {
                    _p.Add(new paragraph() { Attr_type = "checkitem", checkitemID = item.DocID });
                }
                obj._wrapbody = new WrapBody() { _paragraph = _p };
                RIRecordXMLService.Instance.Toxml(obj);

                // //Record转xml数据文件
                MetaXml obj2 = new MetaXml();
                obj2.filename = record.EntityID + "_" + record.Version + "_META";
                obj2.filepath = "crcd";
                obj2._head = new head()
                {
                    documentID = record.DocID + "_META",
                    contentFormat = "doc",
                    contentType = "JP3_RI",
                    documentType = "crcd",
                    effectiveDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    elementID = "1",
                    entityID = record.EntityID,
                    expiredDate = "9999-12-31",
                    subContentFormat = "snapshotFullDocument"
                };

                List<Metadata> _p2 = new List<Metadata>();
                Metadata_Main meta = new Metadata_Main();
                _p2.Add(new Metadata() { Attr_type = "packageID", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(record.PackageID) } });
                _p2.Add(new Metadata() { Attr_type = "class", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(record.Topic) } });
                _p2.Add(new Metadata() { Attr_type = "subclass", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(record.SubTopic) } });
                _p2.Add(new Metadata() { Attr_type = "packageIDwithSuffix", ObjSub = new Metadata_Sub() { text = record.PackageID + ";riallmodule" } });
                _p2.Add(new Metadata() { Attr_type = "documentTitle", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(record.Title) } });
                _p2.Add(new Metadata() { Attr_type = "documentNumber", ObjSub = new Metadata_Sub() { text = "30001010010010" } });
                _p2.Add(new Metadata() { Attr_type = "abstract", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(record.Summary) } });
                _p2.Add(new Metadata() { Attr_type = "riskAbstract", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(record.Risk) } });
                _p2.Add(new Metadata() { Attr_type = "author", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(record.Author) } });
                foreach (var item in record.CheckItem)
                {
                    _p2.Add(new Metadata() { Attr_type = "importance", Attr_citm_id = item.DocID, ObjSub = new Metadata_Sub() { text = item.Importance.ToString() } });
                    _p2.Add(new Metadata() { Attr_type = "hasMeta", Attr_citm_id = item.DocID, ObjSub = new Metadata_Sub() { text = item.HasMeta } });
                    _p2.Add(new Metadata() { Attr_type = "body", Attr_citm_id = item.DocID, Attr_hasMeta = "Y", ObjMain = new Metadata_Main() { _paragraph = new Metadata_paragraph() { Attr_num = "1", markupText = StringHelper.XMLSpecialCharToConvert(item.CheckItemContent), Attr_type = "body" } } });
                    _p2.Add(new Metadata() { Attr_type = "penalty", Attr_citm_id = item.DocID, ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(item.Penalty) } });
                    _p2.Add(new Metadata() { Attr_type = "reasonCode", Attr_citm_id = item.DocID, ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(item.ReasonCodes) } });
                }
                var _listFile = _fileService.GetFilesById(record.DocID);
                foreach (var temp in _listFile)
                {
                    if (temp.MimeType.Equals("url"))
                    {
                        _p2.Add(new Metadata() { Attr_type = "referenceDocuments", Attr_citm_id = record.CheckItem[0].DocID, Attr_doc_id = temp.DocID, Attr_url = temp.Path, Attr_content = "URL", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(temp.Path) } });
                    }
                    if (temp.MimeType.Contains("application"))
                    {
                        var _spt = temp.MimeType.Split('/');
                        var _mimeType = _spt[_spt.Length - 1].ToUpper();
                        _p2.Add(new Metadata() { Attr_type = "referenceDocuments", Attr_citm_id = record.CheckItem[0].DocID, Attr_doc_id = temp.DocID, Attr_content = _mimeType, ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(temp.Title) } });
                    }
                }

                obj2._meta = new Meta() { _metadata = _p2 };
                RIRecordXMLService.Instance.Toxml_Meta(obj2);              
                var _userName = new HttpContextWrapper(HttpContext.Current).User.Identity.Name;
                var _listFileZero = _fileService.GetFilesById("-1",_userName);
                if (_listFileZero != null && _listFileZero.Count > 0)
                {
                    _iRlFileDataProvider.DelFileByDocmentIdForZero(_userName);
                }
                //CheckItem转xml
                foreach (var item in record.CheckItem)
                {
                    //CheckItem描述文件
                    Sub_Xml obj1 = new Sub_Xml();
                    obj1.filename = (Convert.ToInt64(item.EntityID)).ToString() + "_" + item.Version;
                    obj1.filepath = "citm";
                    obj1._head = new head()
                    {
                        documentID = item.DocID,
                        contentFormat = "doc",
                        contentType = "JP3_RI",
                        documentType = "citm",
                        effectiveDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        elementID = "1",
                        entityID = (Convert.ToInt64(item.EntityID)).ToString(),
                        expiredDate = "9999-12-31",
                        subContentFormat = "snapshotFullDocument"
                    };

                    obj1._sub_body = new sub_body()
                    {
                        Attr_type = "body",
                        Attr_num = "1",
                        markupText = item.CheckItemContent
                    };

                    RIRecordXMLService.Instance.Toxml_Sub(obj1);

                    //CheckItem数据文件
                    MetaXml obj3 = new MetaXml();
                    obj3.filename = (Convert.ToInt64(item.EntityID)).ToString() + "_" + item.Version + "_META";
                    obj3.filepath = "citm";
                    obj3._head = new head()
                    {
                        documentID = item.DocID,
                        contentFormat = "doc",
                        contentType = "JP3_RI",
                        documentType = "citm",
                        effectiveDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        elementID = "1",
                        entityID = (Convert.ToInt64(item.EntityID)).ToString(),
                        expiredDate = "9999-12-31",
                        subContentFormat = "snapshotFullDocument"
                    };
                    List<Metadata> _p3 = new List<Metadata>();
                    _p3.Add(new Metadata() { Attr_type = "importance", ObjSub = new Metadata_Sub() { text = item.Importance.ToString() } });
                    _p3.Add(new Metadata() { Attr_type = "hasMeta", ObjSub = new Metadata_Sub() { text = item.HasMeta } });
                    _p3.Add(new Metadata() { Attr_type = "penalty", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(item.Penalty) } });
                    _p3.Add(new Metadata() { Attr_type = "reasonCode", ObjSub = new Metadata_Sub() { text = StringHelper.XMLSpecialCharToConvert(item.ReasonCodes) } });
                    obj3._meta = new Meta() { _metadata = _p3 };
                    RIRecordXMLService.Instance.Toxml_Meta(obj3);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordService>("XML Convert Failed! \n", ex);
                throw new RequestErrorException("Model to XMl is failed__" + ex.Message);
               
            }
        }


        public RIRecordModel CreateRecord(RIRecordModel _model)
        {

            if (_model == null)
            {
                throw new RequestErrorException("-1001");//create record failed
            }
            if (_model.PackageID == null || _model.Topic == null || _model.SubTopic == null)
            {
                throw new RequestErrorException("-1000");//please select catagory
            }
            if (string.IsNullOrEmpty(_model.Author))
            {
                throw new RequestErrorException("Please fill the author");//Please fill the author
            }
            if (_model.Author.Length > 50)
            {
                throw new RequestErrorException("Author is too long");//Please fill the author
            }
            if (_lnRIRecordDataProvider.ExistsRecordTitle(_model.Title))
            {
                throw new RequestErrorException("Please use another title");//already exist title
            }
            _model.DocID = this.GetMaxId().ToString();
            _model.EntityID = (Convert.ToInt64(_model.DocID) - 1).ToString();
            _model.Version = 1;
            _model.IsLatest = true;
            _model.CREATETIME = DateTime.Now;
            _model.UPDATETIME = DateTime.Now;
            _model.EffectiveDate = DateTime.Now;
            _model.Path = _xmlDBSavePath + "crcd" + "/" + (Convert.ToInt64(_model.DocID) - 1).ToString() + "_1.XML";
            _model.MetaPath = _xmlDBSavePath + "crcd" + "/" + (Convert.ToInt64(_model.DocID) - 1).ToString() + "_1_META.XML";

            lnRIRecord.repo.BeginTransaction();

            try
            {
                //var saveResult = _recordService.AddRecord(model);
                var _saveResult = AddRecord(_model, new LnRIRecordEntityModel() { EntityID = _model.EntityID, CreateTime = DateTime.Now, LatestDocID = _model.DocID, Title = _model.Title, Updatetime = DateTime.Now });
                if (_saveResult == null)
                {
                    throw new RequestErrorException("-1001");//create record failed
                }

                List<RICheckItemModel> _rr1 = new List<RICheckItemModel>();
                if (_model.CheckItem != null && _model.CheckItem.Count > 0)
                {
                    _model.CheckItem.ForEach(o =>
                    {
                        o.ParentRecordId = _saveResult.DocID;

                    });

                    _rr1 = CreateCheckItem(_model.CheckItem, new List<RICheckItemModel>() { }, _model.DocID);
                    _model.CheckItem = _rr1;
                }

                MaintainRIRelRecordCheckItem(_rr1, _model.DocID);
                lnRIRecord.repo.CompleteTransaction();
                try
                {
                    if (_model.CheckFiles != null && _model.CheckFiles.Count > 0)
                    {
                        foreach (var fileDoc in _model.CheckFiles)
                        {
                            RIFileModel _newFile = new RIFileModel();
                            string _docid = "";
                            var _OldFile = _fileService.GetFileBydocId(fileDoc);
                            _newFile = _OldFile;
                            _newFile.DocID = System.Guid.NewGuid().ToString("N");
                            _newFile.DocumentID = _model.DocID;
                            _newFile.CreateUser = new HttpContextWrapper(HttpContext.Current).User.Identity.Name;
                            var retFile = _iRlFileDataProvider.CreateFileInfo(_newFile, out _docid);
                        }
                        //_fileService.UpdateFilesByDocumentId(_r1new.CheckFiles, _r1new.DocID);
                    }
                }
                catch (Exception ex)
                {
                    throw new RequestErrorException(ex.Message + "__db for error");
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordService>("CreateRecord Failed! \n", ex);
                lnRIRecord.repo.AbortTransaction();
                throw new RequestErrorException("CreateRecord Failed! No XML generated " + ex.Message);//create record failed
            }
            var _r = _mapperService.MapModel<RIRecordModel>(_model);
            //生成XML
            ToXml(_r);

            return _model;
        }

        public RIRecordModel UpdateRecord(RIRecordModel _record)
        {

            if (_record == null)
            {
                throw new RequestErrorException("-1001");//create record failed
            }
            if (_record.PackageID == null || _record.Topic == null || _record.SubTopic == null)
            {
                throw new RequestErrorException("-1000");//please select catagory
            }
            if (string.IsNullOrEmpty(_record.Author))
            {
                throw new RequestErrorException("Please fill the author");//Please fill the author
            }
            if (_record.Author.Length > 50)
            {
                throw new RequestErrorException("Author is too long");//Please fill the author
            }
            //_record.EntityID = (Convert.ToInt64(_record.DocID) - 1).ToString();
            //_record.Path = _xmlDBSavePath + "crcd" + "/" + (Convert.ToInt64(_record.DocID) - 1).ToString() + "_"+ _record.Version + ".XML";
            //_record.MetaPath = _xmlDBSavePath + "crcd" + "/" + (Convert.ToInt64(_record.DocID) - 1).ToString() + "_"+ _record.Version + "_META.XML";
            //_record.Version = _record.Version;
            //_record.IsLatest = true;
            _record.UPDATETIME = DateTime.Now;

            lnRIRecord.repo.BeginTransaction();

            try
            {
                var updateResult = _lnRIRecordDataProvider.UpdateRecord(_record);

                //var list = _checkItemService.GetCheckItemList(_record.ID);//老状态的checkitem
                //if (list != null && list.Count > 0)
                //{
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        DelteCheckItem(list[i].DocID);
                //        DelteRecordCheckItem(list[i].DocID, _record.ID);
                //    }
                //}
                //CreateCheckItem(_record.CheckItem, _record.ID);
                List<RICheckItemModel> _rr1 = new List<RICheckItemModel>();
                var _oldlist = _checkItemService.GetCheckItemList(_record.DocID);//老状态的checkitem
                _rr1 = CreateCheckItem(_record.CheckItem, _oldlist, _record.ID);

                MaintainRIRelRecordCheckItem(_rr1, _record.ID);
                lnRIRecord.repo.CompleteTransaction();
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordService>("UPdateRecord Failed! \n", ex);
                lnRIRecord.repo.AbortTransaction();
                throw new RequestErrorException("UPdateRecord Failed! No XML generated ");//create record failed
            }
            var _r = _mapperService.MapModel<RIRecordModel>(_record);
            //生成XML
            ToXml(_r);

            return _record;
        }



        private bool DelteCheckItem(string id)
        {
            bool result = _checkItemService.DeleteCheckItemById(id);
            log.Info(u + " DelteCheckItem: " + id);
            return result;
        }

        private bool DelteRecordCheckItem(string id, string _docid)
        {
            bool result = _recordCheckItemService.DeleteRecordCheckItemById(id, _docid);
            log.Info(u + " DelteRecordCheckItem: " + id);
            return result;
        }

        public RIRecordModel SaveNewVersionRecord(RIRecordModel _r1)
        {
            RIRecordModel ret = null;

            LnRIRecordEntityModel _r2 = null;
            RIRecordModel _r1new = new RIRecordModel();

            //_mapperService.MapModel<RIRecordModel>(_r1);//先拷贝一份旧数据
            _r1new = new RIRecordModel()
            {
                ID = _r1.ID,
                DocID = _r1.DocID,
                CheckItem = _r1.CheckItem,
                IsLatest = _r1.IsLatest
            ,
                CheckFiles = _r1.CheckFiles,
                DocumentNumber = _r1.DocumentNumber,
                EntityID = _r1.EntityID,
                Author = _r1.Author,
                EffectiveDate = _r1.EffectiveDate
            ,
                CREATETIME = _r1.CREATETIME,
                IsDelete = _r1.IsDelete,
                MetaPath = _r1.MetaPath,
                PackageID = _r1.PackageID,
                Path = _r1.Path,
                ReferenceDocument = _r1.ReferenceDocument,
                Risk = _r1.Risk,
                SubTopic = _r1.SubTopic
            ,
                Summary = _r1.Summary,
                Title = _r1.Title,
                Topic = _r1.Topic,
                UPDATETIME = DateTime.Now,
                Version = _r1.Version
            };



            lnRIRecord.repo.BeginTransaction();

            try
            {

                int _newversion = _lnRIRecordDataProvider.GetOldVersionRecordMax(_r1) + 1;//1.先保存新的版本号
                _lnRIRecordDataProvider.UpdOldVersionRecord(_mapperService.MapModel<RIRecordModel>(_r1));//2.更新老版本的islast
                string _newPath = _xmlDBSavePath + "crcd" + "/" + _r1.EntityID + "_" + _newversion + ".XML";
                string _newMetaPath = _xmlDBSavePath + "crcd" + "/" + _r1.EntityID + "_" + _newversion + "_META.XML";
                _r1new.Version = _newversion;
                _r1new.DocID = this.GetMaxId().ToString();
                _r1new.UPDATETIME = DateTime.Now;
                _r1new.CREATETIME = DateTime.Now;
                _r1new.Path = _newPath;
                _r1new.MetaPath = _newMetaPath;
                _r1new.IsLatest = true;
                _lnRIRecordDataProvider.AddRecord(_r1new);

                _r2 = new LnRIRecordEntityModel() { EntityID = _r1.EntityID, LatestDocID = _r1new.DocID, Title = _r1.Title, Updatetime = DateTime.Now };
                _lnRIRecordDataProvider.UpdRecordEntity(_r2);//3.更新entity表

                //4.更新checkitem关系表
                List<RICheckItemModel> _rr1 = new List<RICheckItemModel>();
                var _oldlist = _checkItemService.GetCheckItemList(_r1.DocID);//老状态的checkitem


                //List<RICheckItemModel> _delRefCheck = new List<RICheckItemModel>();//包括新checkitem和新版本的checkitem也就是前端传进来docid=空的，在原来的documentid里不应该存在了
                //var q1 = from c in _r1.CheckItem
                //         where string.IsNullOrEmpty(c.DocID)
                //         select new RICheckItemModel { DocID = c.DocID, EntityID = c.EntityID };
                //_delRefCheck = q1.Cast<RICheckItemModel>().ToList<RICheckItemModel>();


                _rr1 = CreateCheckItem(_r1.CheckItem, _oldlist, _r1.ID);

                //MaintainRIRelRecordCheckItem(_rr1, _r1.ID);//5.维护原来的关系

                //6.升版本后把老的documentid的维护关系中排除新的checkitemid,为了不在旧版本的doc里看到新版本的东西
                //foreach (var cc in _delRefCheck)
                //{
                //    //_recordCheckItemService.DeleteRecordCheckItemById(cc.DocID, _r1.ID);
                //}

                MaintainRIRelRecordCheckItem(_rr1, _r1new.DocID);//7.新建升级后版本的关系
                lnRIRecord.repo.CompleteTransaction();
                try
                {
                    if (_r1new.CheckFiles != null && _r1new.CheckFiles.Count > 0)
                    {
                        foreach (var fileDoc in _r1new.CheckFiles)
                        {
                            RIFileModel _newFile = new RIFileModel();
                            string _docid = "";
                            var _OldFile = _fileService.GetFileBydocId(fileDoc);
                            _newFile = _OldFile;
                            _newFile.DocID = System.Guid.NewGuid().ToString("N");
                            _newFile.DocumentID = _r1new.DocID;
                            _newFile.CreateUser = new HttpContextWrapper(HttpContext.Current).User.Identity.Name;
                            var retFile = _iRlFileDataProvider.CreateFileInfo(_newFile, out _docid);
                        }
                        //_fileService.UpdateFilesByDocumentId(_r1new.CheckFiles, _r1new.DocID);
                    }
                }
                catch (Exception ex)
                {
                    throw new RequestErrorException(ex.Message + "__db for error by save new"); ;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordService>("Save new version record Failed! \n", ex);
                lnRIRecord.repo.AbortTransaction();
            }
            //生成XML
            ToXml(_r1new);
            ret = _r1new;

            return ret;
        }


        private List<RICheckItemModel> CreateCheckItem(List<RICheckItemModel> _newR, List<RICheckItemModel> _oldR, string _docmentid = "")
        {
            //int _sortOrder = 0;

            List<RICheckItemModel> _ret = new List<RICheckItemModel>();

            //1.先找出页面(新)数据里不是新加的数据
            var q1 = from c in _newR
                     where !string.IsNullOrEmpty(c.DocID)
                     select c;
            List<RICheckItemModel> _lq1 = q1.Cast<RICheckItemModel>().ToList<RICheckItemModel>();

            //2.找要打删除标签的OldCheckitemRecord from db,从新表里找旧表数据,重复的就是要更新的
            var q2 = from c in _lq1
                     join o in _oldR on new { DocID = c.DocID, EntityID = c.EntityID }
                                                      equals new { DocID = o.DocID, EntityID = o.EntityID }
                     select new RICheckItemModel
                     {
                         DocID = c.DocID,
                         EntityID = c.EntityID,
                         HasMeta = c.HasMeta,
                         CheckItemContent = c.CheckItemContent
                     ,
                         Importance = c.Importance,
                         MetaPath = o.MetaPath,
                         Path = o.Path,
                         Penalty = c.Penalty,
                         ReasonCodes = c.ReasonCodes
                     ,
                         Risk = c.Risk,
                         Title = c.Title,
                         Version = o.Version
                         ,
                         UPDATETIME = DateTime.Now
                         ,
                         IsLatest = o.IsLatest
                         ,
                         IsDelete = o.IsDelete
                         ,
                         UnderReview = c.UnderReview
                     };
            List<RICheckItemModel> _lq2 = q2.Cast<RICheckItemModel>().ToList<RICheckItemModel>();

            foreach (var vv in _lq2)
            {
                _checkItemService.UpdateCheckItem2(vv);//3.更新保存原有的checkitem

                _ret.Add(vv);//保存到返回结果里;
            }
            //4.把需要删除的isdelete=1
            foreach (var vv in _oldR)
            {
                //当前就记录如果不在需要更新的_lq2里，说明是需要删除的
                var q3 = from c in _lq2
                         where c.DocID == vv.DocID && c.EntityID == vv.EntityID
                         select new RICheckItemModel { DocID = c.DocID, EntityID = c.EntityID };
                List<RICheckItemModel> _lq3 = q3.Cast<RICheckItemModel>().ToList<RICheckItemModel>();
                if (_lq3 != null && _lq3.Count == 0)
                {
                    vv.IsDelete = true;
                    _checkItemService.UpdateCheckItem2(vv);
                }
            }



            var q = from p in _newR
                    where string.IsNullOrEmpty(p.DocID)
                    select p;
            List<RICheckItemModel> _lq = q.Cast<RICheckItemModel>().ToList<RICheckItemModel>();


            foreach (var item in _lq)
            {
                if (!string.IsNullOrEmpty(item.EntityID))//说明是升级版本,那需要把之前的underreview全部清空 t549
                {
                    _checkItemService.UpdateCheckitemOldUnderReview(item.EntityID);
                }

                //如果前端传过来的entity是空，则说明这个checkitem是全新的则version=1,不是空的，则说明需要找到当前entity的最大version
                item.Version = string.IsNullOrEmpty(item.EntityID) ? 1 : _checkItemService.GetMaxVersion(item.EntityID) + 1;
                item.DocID = _checkItemService.GetMaxId().ToString();
                //说明是全新的checkitem则生成新的entityid
                item.EntityID = string.IsNullOrEmpty(item.EntityID) ? (Convert.ToInt64(item.DocID) - 1).ToString() : item.EntityID;
                //item.Version = _v;
                item.Path = _xmlDBSavePath + "citm" + "/" + item.EntityID + "_" + item.Version + ".XML";
                item.MetaPath = _xmlDBSavePath + "citm" + "/" + item.EntityID + "_" + item.Version + "_META.XML";


                item.IsLatest = true;


                //把当前entityid的其他老版本的lslatest更新
                _checkItemService.UpdateCheckitemOldVersionLatest(item.EntityID);

                item.IsDelete = false;
                item.Title = "";
                item.EffectiveDate = DateTime.Now;
                item.CREATETIME = DateTime.Now;
                item.UPDATETIME = DateTime.Now;


                var saveResult = _checkItemService.AddCheckItem(item);
                //var saveRecordCheckItem = _recordCheckItemService.AddlnRIRelRecordCheckItem(_recordcheckItem);
                //if (saveResult == null || saveRecordCheckItem == null)
                //{
                //    throw new RequestErrorException("-1003");//CreateCheckItemFailed
                //}
                _ret.Add(item);

            }
            return _ret;
        }

        /// <summary>
        /// 维护document和checkitem的关系表
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_documentid"></param>
        private void MaintainRIRelRecordCheckItem(List<RICheckItemModel> _list, string _documentid)
        {
            //先把原来的documentid的删除
            _recordCheckItemService.DeleteRecordCheckItemByDocment(_documentid);
            var _r = new RecordCheckItemModel();

            int _sort = 0;
            foreach (var e in _list)
            {
                _sort++;
                _r.DocID = _documentid;
                _r.CheckItemID = e.DocID;
                _r.SortOrder = _sort;
                _r.IsDelete = false;
                _recordCheckItemService.AddlnRIRelRecordCheckItem(_r);

            }
            //删除旧版本保留新版本的checkitem
            List<RICheckItemModel> _listislatest0 = _lnRIRecordDataProvider.GetCheckItemListReadytoRef(_documentid);
            foreach (var e1 in _listislatest0)
            {
                _recordCheckItemService.DeleteRecordCheckItemById(e1.DocID, _documentid);
            }
        }


        private void MaintainRIRelRecordCheckItem2(List<RICheckItemModel> _newlist, string _documentid)
        {

            var _r = new RecordCheckItemModel();
            int _sort = 0;
            foreach (var e in _newlist)
            {
                if (e.IsLatest == true)
                {
                    _sort++;
                    _r.DocID = _documentid;
                    _r.CheckItemID = e.DocID;
                    _r.SortOrder = _sort;
                    _r.IsDelete = false;
                    _recordCheckItemService.AddlnRIRelRecordCheckItem(_r);
                }
            }
        }


        public RIRecordModel DeleteRecord(string _documentid)
        {
            if (_documentid == null)
            {
                throw new RequestErrorException("-1001");//create record failed
            }
            lnRIRecord.repo.BeginTransaction();
            RIRecordModel record = new RIRecordModel();
            try
            {
                record = _lnRIRecordDataProvider.GetById(_documentid);
                var list = _checkItemService.GetCheckItemList(_documentid);


                if (record != null)
                {
                    var recordModel = record;
                    recordModel.IsDelete = true;
                    recordModel.UPDATETIME = DateTime.Now;
                    recordModel.IsLatest = false;
                    string _tmpentityid = recordModel.EntityID;
                    recordModel.EntityID = recordModel.EntityID + "99999";


                    var updateResult = _lnRIRecordDataProvider.UpdateRecord(recordModel);

                    //_lnRIRecordDataProvider.DelCheckItem(record.DocID);//先删checkitem
                    _lnRIRecordDataProvider.DelRefCheckItem(record.DocID);//再删维护关系否则维护关系先删就找不到checkitem了

                    ///把最新的record且没删除的islatest=1,前后顺序不能换
                    _lnRIRecordDataProvider.UPDNewRecordEntityIsLast(_tmpentityid);
                    _lnRIRecordDataProvider.UPDOldRecordIsLast(_tmpentityid);
                    _lnRIRecordDataProvider.UPDNewRecordIsLast(_tmpentityid);


                }
                lnRIRecord.repo.CompleteTransaction();
            }
            catch (Exception ex)
            {
                LogHelper.Error<lnRIRecordService>("Delete Record Failed! \n", ex);
                lnRIRecord.repo.AbortTransaction();
                throw new RequestErrorException("Delete Record Failed! " + ex.Message);//create record failed
            }
            return record;
        }
    }
}
