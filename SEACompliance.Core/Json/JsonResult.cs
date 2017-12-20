using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SEACompliance.Core.Json
{
    public class JsonResult<T> : ActionResult
    {
        protected readonly T value;

        protected readonly bool _withDefaultValue;

        public HttpStatusCode StatusCode { get; set; }

        public JsonResult(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.value = value;
            this.StatusCode = HttpStatusCode.OK;
        }
        public JsonResult(T value, bool withDefaultValue) : this(value)
        {
            _withDefaultValue = withDefaultValue;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "text/html; charset=utf-8";
            this.SetStatusCode(response);

            using (JsonTextWriter writer = new JsonTextWriter(response.Output))
            {
                /*JsonSerializer serializer = JsonSerializer.Create(JsonConfig.DefaultSettings);
                serializer.Serialize(writer, this.value);
                 * */
                WriteJson(writer);
            }

        }
        /// <summary>
        /// william song 4/12/2016 
        /// </summary>
        /// <param name="writer"></param>
        private void WriteJson(JsonTextWriter writer)
        {
            JsonSerializerSettings settings = JsonConfig.DefaultSettings;
            var origDefaultValueHandling = settings.DefaultValueHandling;
            var origContractResolver = settings.ContractResolver;
            if (_withDefaultValue)
            {
                settings.DefaultValueHandling = DefaultValueHandling.Include;

                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            JsonSerializer serializer = JsonSerializer.Create(JsonConfig.DefaultSettings);
            serializer.Serialize(writer, this.value);
            if (_withDefaultValue)
            {
                settings.DefaultValueHandling = origDefaultValueHandling;
                settings.ContractResolver = origContractResolver;
            }
        }

        protected virtual void SetStatusCode(HttpResponseBase response)
        {
            if (this.StatusCode != HttpStatusCode.OK)
            {
                int statusCode = (int)this.StatusCode;
                response.StatusCode = statusCode;
                response.StatusDescription = HttpWorkerRequest.GetStatusDescription(statusCode);
            }
        }
    }
}
