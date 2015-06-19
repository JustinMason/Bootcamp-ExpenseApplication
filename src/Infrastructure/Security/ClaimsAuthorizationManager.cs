using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Infrastructure.Security
{
    public class ClaimsAuthorizationManager : System.Security.Claims.ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            Trace.WriteLine("\n\nAuthorizationManager");

            Trace.WriteLine("\nAction:");
            Trace.WriteLine(" " + context.Action.First().Value);

            Trace.WriteLine("\nResources:");
            foreach (var resource in context.Resource)
            {
                Trace.WriteLine(" " + resource.Value);
            }

            Trace.WriteLine("\nClaims:");
            foreach (var claim in context.Principal.Claims)
            {
                Trace.WriteLine(claim.Type + " " + claim.Value);
            }

            return base.CheckAccess(context);

        }

      
    }
}
