using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace SEACompliance.Core.Extensions
{
    public static class ModelStateExtensions
    {
        public static string Error(this ModelStateDictionary modelState)
        {
            string error = string.Empty;
            if (modelState.Values != null && modelState.Values.Count > 0)
            {
                var value = modelState.Values.FirstOrDefault();
                if (value.Errors != null && value.Errors.Count > 0)
                {
                    error = value.Errors.FirstOrDefault().ErrorMessage;
                }
            }
            return error;
        }

        public static object ToClientNotify(this ModelStateDictionary modelState)
        {
            return modelState.ToList().Select(a => new { field = a.Key, errors = a.Value.Errors.Select(c => c.ErrorMessage) });
        }
    }
}
