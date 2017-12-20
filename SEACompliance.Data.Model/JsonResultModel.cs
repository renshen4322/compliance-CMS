using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Model
{
    public class JsonResultModel<T>
    {
        [JsonProperty(PropertyName = "status", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Include)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "code", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Include)]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "data", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Include)]
        public T Data { get; set; }
    }
    
}
