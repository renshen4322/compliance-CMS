using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEACompliance.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RejectJavaScriptAttribute : ValidationAttribute
    {
        private const string errorMessage = "I'm sorry, can't contain special characters";
        private static List<string> array = new List<string>() { "<script>", "</script>", "<frame>", "</frame>", "<iframe>", "</iframe>" };
        public RejectJavaScriptAttribute():base(errorMessage)
        {
        }

        public RejectJavaScriptAttribute(string error):base(error)
        {

        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string s = value.ToString();
                if (!string.IsNullOrEmpty(s))
                {
                    foreach (var item in array)
                    {
                        if (Regex.IsMatch(s,item,RegexOptions.IgnoreCase))
                        {
                            return false;
                        }
                    }                  
                }
            }
            return true;
        }
    }
}
