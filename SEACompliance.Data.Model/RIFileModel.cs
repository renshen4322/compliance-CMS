using Newtonsoft.Json;
using SEACompliance.Core.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RIFileModel
    {
        [JsonProperty(PropertyName = "docId", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Ignore)]
        public string DocID { get; set; }

        [JsonProperty(PropertyName = "title", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ScriptEncodeConverter))]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "mimeType", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Ignore)]
        public string MimeType { get; set; }

        [JsonProperty(PropertyName = "path", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonIgnore]
        public Nullable<System.DateTime> CREATETIME { get; set; }

        [JsonProperty(PropertyName = "content", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ScriptEncodeConverter))]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "documentId", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentID { get; set; }

        [JsonIgnore]
        public Nullable<System.DateTime> UPDATETIME { get; set; }

        [JsonProperty(PropertyName = "fileName", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Ignore)]
        public string FileName { get; set; }

        [JsonIgnore]
        public string CreateUser { get; set; }

        [JsonIgnore]
        public MemoryStream Stream { get; set; }

    }
}
