using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEACompliance.Core.Json
{
    public class ScriptEncodeConverter : JsonConverter
    {
        public bool IsEncode { get; set; }

        public ScriptEncodeConverter()
        {
            IsEncode = true;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(String);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                if (CanConvert(objectType) && IsEncode)
                {
                    if (CanConvert(objectType))
                    {
                        object value = serializer.Deserialize(reader);
                        if (value != null)
                        {
                            string fpattern = @"<script>";
                            string lpattern = @"</script>";
                            value = Regex.Replace(value.ToString(), fpattern, "&lt;script&gt;", RegexOptions.IgnoreCase);
                            value = Regex.Replace(value.ToString(), lpattern, "&lt;/script&gt;", RegexOptions.IgnoreCase);
                        }
                        return value;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            try
            {
                if (value != null && IsEncode)
                {
                    string fpattern = @"<script>";
                    string lpattern = @"</script>";
                    value = Regex.Replace(value.ToString(), fpattern, "&lt;script&gt;", RegexOptions.IgnoreCase);
                    value = Regex.Replace(value.ToString(), lpattern, "&lt;/script&gt;", RegexOptions.IgnoreCase);
                    serializer.Serialize(writer, value);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
