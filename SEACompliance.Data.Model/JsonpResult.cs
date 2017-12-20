using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace SEACompliance.Model
{
    public class JsonpResult : JsonResult
    {
        private static readonly string JsonpCallbackName = "callback";
        private static readonly string CallbackApplicationType = "application/json";
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                new ArgumentNullException("context");
            }
            ContentType = CallbackApplicationType;

            var req = context.HttpContext.Request;
            var res = context.HttpContext.Response;
            if (Data != null && req[JsonpCallbackName] != null)
            {
                string result = string.Format("{0}({1})", req[JsonpCallbackName], JsonConvert.SerializeObject(Data));
                res.Write(result);
            }
            else
            {
                base.ExecuteResult(context);
            }

        }
    }
}