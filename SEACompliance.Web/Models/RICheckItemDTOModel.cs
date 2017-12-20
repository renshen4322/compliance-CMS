using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEACompliance.Web.Models
{
    public class RICheckItemDTOModel
    {
        public string ParentRecordId { get; set; }
        public int ItemID { get; set; }
        public string ID { get; set; }
        public string DocID { get; set; }
        public string EntityID { get; set; }
        public int Version { get; set; }
        public bool IsLatest { get; set; }
        public DateTime? CREATETIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
        public string Path { get; set; }
        public bool IsDelete { get; set; }
        public string MetaPath { get; set; }
        public string Title { get; set; }
        public string CheckItemContent { get; set; }
        public string Penalty { get; set; }
        public string ReasonCodes { get; set; }
        public int Importance { get; set; }
        public int Risk { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string HasMeta { get; set; }
        public int? UnderReview { get; set; }
    }
}