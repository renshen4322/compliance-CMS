using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using SEACompliance.Service.Interface;
using SEACompliance.Core.Autofac;
using SEACompliance.Core.Extensions;
using SEACompliance.Model;
using SEACompliance.DataBase;
using SEACompliance.Core.Json;
using SEACompliance.Service;
using SEACompliance.Web.Models;
using AutoMapper;

namespace SEACompliance.Web.Controllers
{
    [PluginController("SEACompliance")]
    public class RICheckItemApiController : UmbracoAuthorizedJsonController
    {
        private IlnRICheckItemService _CheckItemService;
        private IMapperService _mapperService;

        public RICheckItemApiController(IlnRICheckItemService rICheckItemService, IMapperService mapperService)
        {
            _CheckItemService = rICheckItemService;// ContainerManager.Resolve<IlnRICheckItemService>();
            _mapperService = mapperService;// ContainerManager.Resolve<IMapperService>();
        }
        
        [HttpGet]
        public JsonResultModel<RICheckItemDTOModel> GetById(string id)
        {
            var result = new JsonResultModel<RICheckItemDTOModel> { Status = JsonResponseStatus.Failed };
            var CheckItem = _CheckItemService.GetById(id);
            var CheckItemViewModel = _mapperService.MapModel<RICheckItemDTOModel>(CheckItem);

            if (CheckItem != null)
            {
                result.Status = JsonResponseStatus.Success;
                result.Data = CheckItemViewModel;
            }
            return result;
        }
        
    }
}