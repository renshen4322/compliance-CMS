using AutoMapper;
using SEACompliance.DataBase;
using SEACompliance.Model;
using SEACompliance.Web.Models;
using System.Collections.Generic;
using Umbraco.Core.Persistence;

namespace SEACompliance.Web
{
    public class BackOfficeMappingConfig
    {
        public static void RegisterMapping()
        {
            Mapper.CreateMap<lnRIRecord, RIRecordModel>();
            Mapper.CreateMap<RIRecordModel, lnRIRecord>();
            Mapper.CreateMap<IEnumerable<lnRIRecord>, IEnumerable<RIRecordModel>>();
            Mapper.CreateMap<IEnumerable<RIRecordModel>, IEnumerable<lnRIRecord>>();
            Mapper.CreateMap<Page<lnRIRecord>, PageModel<RIRecordModel>>();
            Mapper.CreateMap<PageModel<RIRecordModel>, Page<lnRIRecord>>();

            Mapper.CreateMap<RIRecordModel, RIRecordDTOModel>();
            Mapper.CreateMap<RIRecordDTOModel, RIRecordModel>();
            Mapper.CreateMap<IEnumerable<RIRecordModel>, IEnumerable<RIRecordDTOModel>>();
            Mapper.CreateMap<IEnumerable<RIRecordDTOModel>, IEnumerable<RIRecordModel>>();
            Mapper.CreateMap<Page<RIRecordModel>, PageModel<RIRecordDTOModel>>();
            Mapper.CreateMap<PageModel<RIRecordDTOModel>, Page<RIRecordModel>>();

            Mapper.CreateMap<lnRICheckItem, RICheckItemModel>();
            Mapper.CreateMap<RICheckItemModel, lnRICheckItem>();
            Mapper.CreateMap<IEnumerable<lnRICheckItem>, IEnumerable<RICheckItemModel>>();
            Mapper.CreateMap<IEnumerable<RICheckItemModel>, IEnumerable<lnRICheckItem>>();

            Mapper.CreateMap<RICheckItemModel, RICheckItemDTOModel>();
            Mapper.CreateMap<RICheckItemDTOModel, RICheckItemModel>();           
            Mapper.CreateMap<IEnumerable<RICheckItemModel>, IEnumerable<RICheckItemDTOModel>>();
            Mapper.CreateMap<IEnumerable<RICheckItemDTOModel>, IEnumerable<RICheckItemModel>>();

            Mapper.CreateMap<lnRIRelRecordCheckItem, RecordCheckItemModel>();
            Mapper.CreateMap<RecordCheckItemModel, lnRIRelRecordCheckItem>();
            Mapper.CreateMap<IEnumerable<lnRIRelRecordCheckItem>, IEnumerable<RecordCheckItemModel>>();
            Mapper.CreateMap<IEnumerable<RecordCheckItemModel>, IEnumerable<lnRIRelRecordCheckItem>>();

            Mapper.CreateMap<RecordCheckItemModel, RecordCheckItemDTOModel>();
            Mapper.CreateMap<RecordCheckItemDTOModel, RecordCheckItemModel>();
            Mapper.CreateMap<IEnumerable<RecordCheckItemModel>, IEnumerable<RecordCheckItemDTOModel>>();
            Mapper.CreateMap<IEnumerable<RecordCheckItemDTOModel>, IEnumerable<RecordCheckItemModel>>();


            Mapper.CreateMap<LnRIRecordEntityModel, lnRIRecordEntity>();
            Mapper.CreateMap<lnRIRecordEntity, LnRIRecordEntityModel>();
                      
       

            Mapper.CreateMap<lnRIFile, RIFileModel>();
            Mapper.CreateMap<RIFileModel, lnRIFile>();
            Mapper.CreateMap<IEnumerable<lnRIFile>, IEnumerable<RIFileModel>>();
            Mapper.CreateMap<IEnumerable<RIFileModel>, IEnumerable<lnRIFile>>();

            Mapper.CreateMap<RIFileModel, RIFileDTOModel>();
            Mapper.CreateMap<RIFileDTOModel, RIFileModel>();
            Mapper.CreateMap<IEnumerable<RIFileModel>, IEnumerable<RIFileDTOModel>>();
            Mapper.CreateMap<IEnumerable<RIFileDTOModel>, IEnumerable<RIFileModel>>();//

            Mapper.CreateMap<lnRIFileContent, RIFileContentModel>();
            Mapper.CreateMap<RIFileContentModel, lnRIFileContent>();
            Mapper.CreateMap<IEnumerable<lnRIFileContent>, IEnumerable<RIFileContentModel>>();
            Mapper.CreateMap<IEnumerable<RIFileContentModel>, IEnumerable<lnRIFileContent>>();

            Mapper.CreateMap<lnRIRelRecordTool, RIRelRecordToolModel>();
            Mapper.CreateMap<RIRelRecordToolModel, lnRIRelRecordTool>();
            Mapper.CreateMap<IEnumerable<lnRIRelRecordTool>, IEnumerable<RIRelRecordToolModel>>();
            Mapper.CreateMap<IEnumerable<RIRelRecordToolModel>, IEnumerable<lnRIRelRecordTool>>();

        }


    }
}

