using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using Bootcamp.Infrastructure.Data;

namespace Bootcamp.Infrastructure.Security
{
    public class ClaimsAuthenticationManagerHttpModule : IHttpModule
    {
        private readonly IMetaUserRepository _metaUserRepository;

        public ClaimsAuthenticationManagerHttpModule(IMetaUserRepository metaUserRepository)
        {
            _metaUserRepository = metaUserRepository;
        }

        public void Dispose()
        { }

        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += Context_PostAuthenticateRequest;
        }

        void Context_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;

            // no need to call transformation if session already exists
            if (FederatedAuthentication.SessionAuthenticationModule != null &&
                FederatedAuthentication.SessionAuthenticationModule.ContainsSessionTokenCookie(context.Request.Cookies))
            {
                return;
            }

            var transformer = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager as ClaimsAuthenticationManager;
            
            if (transformer != null)
            {
                transformer.SetMetaUserRepository(_metaUserRepository);
                var transformedPrincipal = transformer.Authenticate(context.Request.RawUrl, context.User as ClaimsPrincipal);

                context.User = transformedPrincipal;
                Thread.CurrentPrincipal = transformedPrincipal;
            }

        }
    }
}