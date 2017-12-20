using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Core.Logging;
using SEACompliance.Service.Interface;
using SEACompliance.Core.Autofac;
using SEACompliance.Core.Extensions;
using SEACompliance.Model;
using SEACompliance.DataBase;
using SEACompliance.Core.Json;
using SEACompliance.Service;
using SEACompliance.Model.RIRecordXMLModel;
using log4net;
using Umbraco.Web;
using SEACompliance.Core.Common;
using AutoMapper;
using SEACompliance.Web.Models;
using SEACompliance.Core.ExceptionApi;
using SEACompliance.Web.Filters;

namespace SEACompliance.Web.Controllers
{

    [PluginController("SEACompliance")]
    public class RIRecordApiController : UmbracoAuthorizedJsonController
    {
        private IlnRIRecordService _recordService;
        private IMapperService _mapperService;
        private IlnRICheckItemService _checkItemService;
        private IlnRIRelRecordCheckItemService _recordCheckItemService;
        private IRIRecordTypeService _recordtypeService;
       
        ILog log = log4net.LogManager.GetLogger("RIRecordApiController_Info");
        string u = UmbracoContext.Current.Security.CurrentUser != null ? UmbracoContext.Current.Security.CurrentUser.Username.ToString() : string.Empty;

        private static string _pa = System.Configuration.ConfigurationManager.AppSettings["XMLPath"].ToString();
        private string Path = "/" + _pa + "/";
        private string XmlDBSavePath = System.Configuration.ConfigurationManager.AppSettings["XMLUrlPath"].ToString();
        public RIRecordApiController(IlnRIRecordService rIRecordService, IMapperService mapperService, IlnRICheckItemService rICheckItemService, IlnRIRelRecordCheckItemService rIRelRecordCheckItemService, IRIRecordTypeService rIRecordTypeService)
        {
            _recordService = rIRecordService;//ContainerManager.Resolve<IlnRIRecordService>();
            _mapperService = mapperService;// ContainerManager.Resolve<IMapperService>();
            _checkItemService = rICheckItemService;// ContainerManager.Resolve<IlnRICheckItemService>();
            _recordCheckItemService = rIRelRecordCheckItemService;//ContainerManager.Resolve<IlnRIRelRecordCheckItemService>();
            _recordtypeService = rIRecordTypeService;//ContainerManager.Resolve<IRIRecordTypeService>();
           
        }




        [HttpPost]
        public JsonResultModel<PageModel<RIRecordDTOModel>> RecordByKeyWordWithPage(PageModel<RIRecordDTOModel> pageModel)
        {
            var result = new JsonResultModel<PageModel<RIRecordDTOModel>> { Status = JsonResponseStatus.Failed };
            try
            {
                if (pageModel == null)
                {
                    result.Code = JsonResponseCode.InvalidParameter;
                    return result;
                }
                List<RIRecordModel> _items = Mapper.Map<List<RIRecordModel>>(pageModel.Items);
                PageModel<RIRecordModel> returnPagerModel = new PageModel<RIRecordModel> { CurrentPage = pageModel.CurrentPage, ItemsPerPage = pageModel.ItemsPerPage, Items = _items };
                returnPagerModel = _recordService.GetRecordByKeyWordWithPage(returnPagerModel, pageModel.KeyWord);
                List<RIRecordDTOModel> _itemsDTO = Mapper.Map<List<RIRecordDTOModel>>(returnPagerModel.Items);
                pageModel = new PageModel<RIRecordDTOModel> { CurrentPage = returnPagerModel.CurrentPage, Items = _itemsDTO, ItemsPerPage = returnPagerModel.ItemsPerPage, TopicId = returnPagerModel.TopicId, TotalItems = returnPagerModel.TotalItems, TotalPages = returnPagerModel.TotalPages };
                if (returnPagerModel != null)
                {
                    result.Status = JsonResponseStatus.Success;
                    result.Data = pageModel;
                }
                else
                {
                    result.Code = JsonResponseCode.GetRecordByKeyWordWithPageFailed;
                }
            }
            catch (Exception ex)
            {
                result.Code = JsonResponseCode.GetRecordByKeyWordWithPageFailed;

            }
            return result;
        }


        [HttpPost]
        public JsonResultModel<RIRecordDTOModel> CreateRecord(RIRecordDTOModel _record)
        {
            var result = new JsonResultModel<RIRecordDTOModel> { Status = JsonResponseStatus.Failed };
            try
            {

                var model = _mapperService.MapModel<RIRecordModel>(_record);

                var saveResult = _recordService.CreateRecord(model);

                result.Data = _mapperService.MapModel<RIRecordDTOModel>(saveResult);
                result.Status = JsonResponseStatus.Success;
                log.Info(u + " CreateRecord: " + JsonSerialization.ObjectToJson(result.Data));

            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Failed;
                result.Code = ex.Message;
                return result;
            }
            return result;
        }


        [HttpPost]
        public JsonResultModel<RIRecordDTOModel> CreateRecordNewVersion(RIRecordDTOModel _record)
        {
            var result = new JsonResultModel<RIRecordDTOModel> { Status = JsonResponseStatus.Failed };
            try
            {

                var model = _mapperService.MapModel<RIRecordModel>(_record);

                var saveResult = _recordService.SaveNewVersionRecord(model);

                result.Data = _mapperService.MapModel<RIRecordDTOModel>(saveResult);
                result.Status = JsonResponseStatus.Success;
                log.Info(u + " SaveNewVersionRecord : " + JsonSerialization.ObjectToJson(result.Data));

            }
            catch (Exception ex)
            {
                result.Status = JsonResponseStatus.Failed;
                result.Code = ex.Message;
                return result;
            }
            return result;
        }


        [HttpGet]
        public JsonResultModel<RIRecordDTOModel> GetById(string id)
        {

            var result = new JsonResultModel<RIRecordDTOModel> { Status = JsonResponseStatus.Failed };
            var record = _recordService.GetById(id);
            var recordViewModel = _mapperService.MapModel<RIRecordDTOModel>(record);

            var checkItemList = _checkItemService.GetCheckItemList(id);
            if (checkItemList != null && checkItemList.Count > 0)
            {
                var checkItemViewModel = _mapperService.MapModel<List<RICheckItemModel>, List<RICheckItemDTOModel>>(checkItemList);
                recordViewModel.CheckItem = checkItemViewModel;
            }
            if (record != null)
            {
                result.Status = JsonResponseStatus.Success;
                result.Data = recordViewModel;
            }
            return result;
        }

        [HttpPost]
        public JsonResultModel<RIRecordDTOModel> UpdateRecord(RIRecordDTOModel record)
        {
            var result = new JsonResultModel<RIRecordDTOModel> { Status = JsonResponseStatus.Failed };
            try
            {

                var updateRecord = _mapperService.MapModel<RIRecordModel>(record);

                var updateResult = _recordService.UpdateRecord(updateRecord);
                if (updateResult == null)
                {
                    throw new RequestErrorException(JsonResponseCode.UpdateRecordFailed);
                }
                result.Status = JsonResponseStatus.Success;
                log.Info(u + " UpdateRecord: " + JsonSerialization.ObjectToJson(record));

            }
            catch (Exception ex)
            {
                result.Code = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public JsonResultModel<RIRecordDTOModel> GetDeleteRecordById(string Id)
        {
            var result = new JsonResultModel<RIRecordDTOModel> { Status = JsonResponseStatus.Failed };
            try
            {
                _recordService.DeleteRecord(Id);

                log.Info(u + " DeleteRecord: " + Id);
                result.Status = JsonResponseStatus.Success;
            }
            catch (Exception ex)
            {
                result.Code = ex.Message;
            }
            return result;
        }

        private bool CheckRecordIsExists(RIRecordDTOModel record)
        {
            bool result = _recordService.ExistsRecord(record.DocID, record.ID);
            return result;
        }



        /// <summary>
        /// Module分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResultModel<List<RIRecord_Type>> GetAllType_Module()
        {
            var result = new JsonResultModel<List<RIRecord_Type>> { Status = JsonResponseStatus.Failed };
            var record = _recordtypeService.GetAll_Module();
            if (record != null && record.Count > 0)
            {

                result.Status = JsonResponseStatus.Success;
                result.Data = record;
            }
            return result;
        }

        /// <summary>
        /// Topic分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResultModel<List<RIRecord_Type>> GetType_Topic(string pcode)
        {
            var result = new JsonResultModel<List<RIRecord_Type>> { Status = JsonResponseStatus.Failed };
            var record = _recordtypeService.GetSome_Topic(pcode);
            if (record != null && record.Count > 0)
            {

                result.Status = JsonResponseStatus.Success;
                result.Data = record;
            }
            return result;
        }

        /// <summary>
        /// Sub分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResultModel<List<RIRecord_Type>> GetType_Sub(string pcode)
        {
            var result = new JsonResultModel<List<RIRecord_Type>> { Status = JsonResponseStatus.Failed };
            var record = _recordtypeService.GetSome_Sub(pcode);
            if (record != null && record.Count > 0)
            {

                result.Status = JsonResponseStatus.Success;
                result.Data = record;
            }
            return result;
        }

        /// <summary>
        /// 所有分类数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResultModel<List<RIRecord_Type>> GetAllType()
        {
            var result = new JsonResultModel<List<RIRecord_Type>> { Status = JsonResponseStatus.Failed };

            var record = _recordtypeService.GetAll();
            if (record != null && record.Count > 0)
            {

                result.Status = JsonResponseStatus.Success;
                result.Data = record;
            }

            return result;
        }


        [HttpPost]
        public JsonResultModel<RIRecord_Type> AddorUpdateRecordType(RIRecord_Type record)
        {


            var result = new JsonResultModel<RIRecord_Type> { Status = JsonResponseStatus.Failed };
            try
            {

                if (record == null)
                {
                    result.Code = JsonResponseCode.InvalidParameter;
                    return result;
                }

                RIRecord_Type updateResult = null;

                if (record.Id != 0)
                {
                    updateResult = _recordtypeService.UpdateRecordType(record);
                }
                else
                {
                    updateResult = _recordtypeService.AddRecordType(record);
                }



                if (updateResult != null)
                {
                    result.Status = JsonResponseStatus.Success;
                    result.Data = record;
                    log.Info(u + " UpdateRecordtype: " + JsonSerialization.ObjectToJson(record));
                    return result;
                }
                else
                {
                    result.Code = JsonResponseCode.UpdateRecordFailed;
                    return result;
                }
            }
            catch
            {
                result.Code = JsonResponseCode.UpdateRecordFailed;
                return result;
            }
        }


        [HttpGet]
        public JsonResultModel<bool> GetDeleteRecordType(int Id)
        {
            var result = new JsonResultModel<bool> { Status = JsonResponseStatus.Failed };
            try
            {


                var updateResult = _recordtypeService.DeleteRecordType(Id);

                if (updateResult)
                {
                    result.Status = JsonResponseStatus.Success;
                    log.Info(u + " DelRecordtype: " + JsonSerialization.ObjectToJson(Id));
                    return result;
                }
                else
                {
                    result.Code = JsonResponseCode.UpdateRecordFailed;
                    return result;
                }
            }
            catch
            {
                result.Code = JsonResponseCode.UpdateRecordFailed;
                return result;
            }
        }

    }
}