using SEACompliance.Core.ConfigurationManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;

namespace SEACompliance.Core.Web
{
    public static class HttpContextManager
    {

        private const string Key = "userinfo";
        public static LoginUserInfo GetUserInfo(HttpContextBase httpContext)
        {
            LoginUserInfo userinfo = (LoginUserInfo)httpContext.Items[Key];
            if (userinfo == null)
            {
                if (httpContext.User != null && httpContext.Request.IsAuthenticated)
                {
                    var fi = (FormsIdentity)httpContext.User.Identity;
                    var ticket = fi.Ticket; //get ticket
                    var userData = ticket.UserData;

                    userinfo = JsonConvert.DeserializeObject<LoginUserInfo>(userData);
                    httpContext.Items[Key] = userinfo;

                }
            }

            return userinfo;
        }

        public static LoginUserInfo GetUserInfo(System.Web.HttpContext httpContext)
        {
            LoginUserInfo userinfo = (LoginUserInfo)httpContext.Items[Key];
            if (userinfo == null)
            {
                if (httpContext.User != null && httpContext.Request.IsAuthenticated)
                {
                    var fi = (FormsIdentity)httpContext.User.Identity;
                    var ticket = fi.Ticket; //get ticket
                    var userData = ticket.UserData;

                    userinfo = JsonConvert.DeserializeObject<LoginUserInfo>(userData);
                    httpContext.Items[Key] = userinfo;

                }
            }

            return userinfo;
        }

        public static LoginUserInfo GetUserInfo()
        {
            return GetUserInfo(System.Web.HttpContext.Current);
        }

        public static void HandleUseSSL()
        {
            var context = HttpContext.Current;
            if (!context.Request.IsSecureConnection && Convert.ToBoolean(ComplianceConfigurationManager.GetString("ComplianceUseSSL")))
            {
                var secureUrlBuilder = new UriBuilder(new Uri(context.Request.Url, context.Request.RawUrl))
                {
                    Scheme = Uri.UriSchemeHttps
                };

                var httpsPort = ComplianceConfigurationManager.GetString("HttpsPort");
                secureUrlBuilder.Port = string.IsNullOrEmpty(httpsPort) ? 443 : Convert.ToInt32(httpsPort);

                context.Response.Redirect(secureUrlBuilder.Uri.AbsoluteUri);
            }
        }
    }

    [Serializable]
    public class LoginUserInfo
    {
        public int MemberId { get; set; }

        public string Email { get; set; }

        public bool ServiceAvaiable { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> Features { get; set; }

    }
}
