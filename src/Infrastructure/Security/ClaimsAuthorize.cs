using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Bootcamp.Infrastructure.Security
{
    public class ClaimsAuthorize : AuthorizeAttribute
    {
        private readonly string _action;
        private readonly string[] _resources;
        private const string ClaimsAuthorizeKey = "Bootcamp.Infrastructure.Security.ClaimsAuthorize";

        public ClaimsAuthorize()
        { }

        public ClaimsAuthorize(ClaimsAction action, params string[] resources)
        {
            _action = action.ToString();
            _resources = resources;
        }

        public ClaimsAuthorize(string action, params string[] resources)
        {
            _action = action;
            _resources = resources;
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Items[ClaimsAuthorizeKey] = filterContext;
            base.OnAuthorization(filterContext); 
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var principal = httpContext.User as ClaimsPrincipal;

            if (principal == null || principal.Identity == null)
            {
                principal = new ClaimsPrincipal(new ClaimsIdentity());
            }

            if (!string.IsNullOrWhiteSpace(_action))
            {
                return ClaimsAuthorization.CheckAccess(principal, _action, _resources);
            }
            else
            {
                var filterContext = httpContext.Items[ClaimsAuthorizeKey] as System.Web.Mvc.AuthorizationContext;
                return CheckAccess(principal, filterContext);
            }
        }

        protected virtual bool CheckAccess(ClaimsPrincipal principal, System.Web.Mvc.AuthorizationContext filterContext)
        {
            var action = filterContext.RouteData.Values["action"] as string;
            var controller = filterContext.RouteData.Values["controller"] as string;

            return ClaimsAuthorization.CheckAccess(principal, action, controller);
        }
    }
}
