using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Web;
using Bootcamp.Infrastructure.Data;

namespace Bootcamp.Infrastructure.Security
{
    public class ClaimsAuthenticationManager : System.Security.Claims.ClaimsAuthenticationManager
    {

        private IMetaUserRepository _metaUserRepository;

        public void SetMetaUserRepository(IMetaUserRepository metaUserRepository)
        {
            _metaUserRepository = metaUserRepository;
        }

        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return incomingPrincipal;
            }

           //TODO: Access MetaDepot and Create a new ClaimsPrincipal that has the claims associated with the configured Roles
            var newPrincipal = Transform(incomingPrincipal);

            EstablishSession(newPrincipal);

            return newPrincipal;
        }

        ClaimsPrincipal Transform(ClaimsPrincipal incomingPrincipal)
        {
            var nameClaim = incomingPrincipal.Identities.First().FindFirst(ClaimTypes.Name);

            if (incomingPrincipal.Identity is WindowsIdentity)
            {
                var account = new NTAccount(incomingPrincipal.Identity.Name);
            }

            //TODO: Stradegy to Convert Existing Roles into Claims
            var existingMetaUserRoles = _metaUserRepository.GetApplicationRolesByUserName(nameClaim.Value);
      
            var claims = new List<Claim>
            {
                nameClaim,
                new Claim(ClaimTypes.Email, "foo@thinktecture.com"),
                new Claim("http://claims/time", DateTime.Now.ToLongTimeString())
            };

            var id = new ClaimsIdentity(claims, "Application");
            return new ClaimsPrincipal(id);
        }

        private void EstablishSession(ClaimsPrincipal principal)
        {
            if (HttpContext.Current != null)
            {
                var sessionToken = new SessionSecurityToken(principal,TimeSpan.FromMinutes(5));
                FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(sessionToken);
            }
        }
    }
}
