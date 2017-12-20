using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Model
{
    public class PageModel<T>
    {
        public long CurrentPage { get; set; }

        public List<T> Items { get; set; }

        public long ItemsPerPage { get; set; }

        public long TotalItems { get; set; }

        public long TotalPages { get; set; }

        public string KeyWord { get; set; }

        public int TopicId { get; set; }

        public int ParentObligationQuestionId { get; set; }

        public string ReportType { get; set; }

        public string Id { get; set; }

        public string Status { get; set; }
    }
}
