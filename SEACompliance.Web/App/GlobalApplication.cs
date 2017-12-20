using SEACompliance.Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace SEACompliance.Web.App
{
    public class GlobalApplication : Umbraco.Web.UmbracoApplication
    {

        private void Application_BeginRequest(Object sender, EventArgs e)
        {
            HttpContextManager.HandleUseSSL();
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);

            }
            //else
            //{
            //    HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower().StartsWith("~/umbraco/");
            //}
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.ToLower().StartsWith("~/umbraco/api/");
        }
    }
}
