using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Model
{
    public class LnRIRecordEntityModel
    {
        public string EntityID { get; set; }
        public string Title { get; set; }
        public string LatestDocID { get; set; }
        public System.DateTime? CreateTime { get; set; }
        public System.DateTime? Updatetime { get; set; }
    }
}
