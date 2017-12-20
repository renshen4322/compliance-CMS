using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEACompliance.Web.Models
{
    public class RecordCheckItemDTOModel
    {
        public string DocID { get; set; }
        public string CheckItemID { get; set; }
        public int SortOrder { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}