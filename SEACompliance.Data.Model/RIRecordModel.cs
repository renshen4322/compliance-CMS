using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Model
{
    public class RIRecordModel
    {
        public string ID { get; set; }
        public string DocID { get; set; }
        public string Title { get; set; }
        public string PackageID { get; set; }
        public string Topic { get; set; }
        public string SubTopic { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        public string Risk { get; set; }
        public string ReferenceDocument { get; set; }
        public string EntityID { get; set; }
        public int Version { get; set; }
        public bool IsLatest { get; set; }
        public DateTime? CREATETIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
        public string Path { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string MetaPath { get; set; }
        public string DocumentNumber { get; set; }
        public List<RICheckItemModel> CheckItem { get; set; }
        public List<string> CheckFiles { get; set; }
    }
}
