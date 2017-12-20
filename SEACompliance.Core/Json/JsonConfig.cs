using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SEACompliance.Core.Json
{
    public class JsonConfig
    {
        private static JsonSerializerSettings s_defaultSettings;

        public static JsonSerializerSettings DefaultSettings
        {
            get
            {

                if (s_defaultSettings == null)
                {
                    s_defaultSettings = new JsonSerializerSettings()
                    {
                        ContractResolver = (IContractResolver)new DefaultContractResolver(),
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                }
                return s_defaultSettings;
            }
        }
            


    }
}
