using System;
using System.Linq;
using System.Web;

using Hangfire.Annotations;
using Hangfire.Dashboard;
using Umbraco.Core;
using Umbraco.Core.Security;

namespace SEACompliance.Web.Filters
{
    public class UmbracoUserAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize([NotNull] DashboardContext context)
        {
            var auth = new HttpContextWrapper(HttpContext.Current).GetUmbracoAuthTicket();
            if (auth == null)
            {
                return false;
            }

            var backofficeUser = ApplicationContext.Current.Services.UserService.GetByUsername(auth.Name);
            var isAllowed = backofficeUser != null &&
                backofficeUser.AllowedSections.Any(x =>
                    x.Equals("developer", StringComparison.InvariantCultureIgnoreCase) || x.Equals("jobs", StringComparison.InvariantCultureIgnoreCase)
                    );
            return isAllowed;
        }

    }
}