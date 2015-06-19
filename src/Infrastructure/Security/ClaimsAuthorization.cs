﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Bootcamp.Infrastructure.Security
{
    /// <summary>
    /// Provides direct access methods for evaluating authorization policy
    /// </summary>
    public static class ClaimsAuthorization
    {
        /// <summary>
        /// Default action claim type.
        /// </summary>
        public const string ActionType = "http://application/claims/authorization/action";

        /// <summary>
        /// Default resource claim type
        /// </summary>
        public const string ResourceType = "http://application/claims/authorization/resource";

        public static bool EnforceAuthorizationManagerImplementation { get; set; }

        public static System.Security.Claims.ClaimsAuthorizationManager CustomAuthorizationManager { get; set; }

        static readonly Lazy<System.Security.Claims.ClaimsAuthorizationManager> ClaimsAuthorizationManager = new Lazy<System.Security.Claims.ClaimsAuthorizationManager>(()
            => new IdentityConfiguration().ClaimsAuthorizationManager);

        /// <summary>
        /// Gets the registered authorization manager.
        /// </summary>
        public static System.Security.Claims.ClaimsAuthorizationManager AuthorizationManager
        {
            get
            {
                if (CustomAuthorizationManager != null)
                {
                    return CustomAuthorizationManager;
                }

                return ClaimsAuthorizationManager.Value;
            }
        }

        static ClaimsAuthorization()
        {
            EnforceAuthorizationManagerImplementation = true;
        }

        /// <summary>
        /// Checks the authorization policy.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="action">The action.</param>
        /// <returns>true when authorized, otherwise false</returns>
        public static bool CheckAccess(string action, params string[] resources)
        {
            Contract.Requires(!String.IsNullOrEmpty(action));


            return CheckAccess(ClaimsPrincipal.Current, action, resources);
        }

        public static bool CheckAccess(ClaimsPrincipal principal, string action, params string[] resources)
        {
            var context = CreateAuthorizationContext(
                principal,
                action,
                resources);

            return CheckAccess(context);
        }

        public static bool CheckAccess(IPrincipal principal, string action, params string[] resources)
        {
            var claimsPrincipal = principal as ClaimsPrincipal;
            if (claimsPrincipal == null)
            {
                throw new InvalidOperationException("Principal is not a ClaimsPrincipal");
            }

            return CheckAccess(claimsPrincipal, action, resources);
        }


        /// <summary>
        /// Checks the authorization policy.
        /// </summary>
        /// <param name="actions">The actions.</param>
        /// <param name="resources">The resources.</param>
        /// <returns>true when authorized, otherwise false</returns>
        public static bool CheckAccess(Collection<Claim> actions, Collection<Claim> resources)
        {
            Contract.Requires(actions != null);
            Contract.Requires(resources != null);


            return CheckAccess(new AuthorizationContext(
                ClaimsPrincipal.Current, resources, actions));
        }

        /// <summary>
        /// Checks the authorization policy.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="resources">The resources.</param>
        /// <returns>true when authorized, otherwise false</returns>
        public static bool CheckAccess(string action, params Claim[] resources)
        {
            Contract.Requires(action != null);
            Contract.Requires(resources != null);

            var actionCollection = new Collection<Claim>();
            actionCollection.Add(new Claim(ActionType, action));
            var resourceCollection = new Collection<Claim>();
            foreach (var resource in resources) resourceCollection.Add(resource);

            return CheckAccess(new AuthorizationContext(
                ClaimsPrincipal.Current, resourceCollection, actionCollection));
        }

        /// <summary>
        /// Checks the authorization policy.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="resource">The resource name.</param>
        /// <param name="resources">The resources.</param>
        /// <returns>true when authorized, otherwise false</returns>
        public static bool CheckAccess(string action, string resource, params Claim[] resources)
        {
            Contract.Requires(action != null);
            Contract.Requires(resource != null);

            var resourceList = resources.ToList();
            resourceList.Add(new Claim(ResourceType, resource));
            return CheckAccess(action, resourceList.ToArray());
        }

        /// <summary>
        /// Checks the authorization policy.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <returns>true when authorized, otherwise false</returns>
        public static bool CheckAccess(AuthorizationContext context)
        {
            Contract.Requires(context != null);


            if (EnforceAuthorizationManagerImplementation)
            {
                var authZtype = AuthorizationManager.GetType().FullName;
                if (authZtype.Equals("System.Security.Claims.ClaimsAuthorizationManager"))
                {
                    throw new InvalidOperationException("No ClaimsAuthorizationManager implementation configured.");
                }
            }

            return AuthorizationManager.CheckAccess(context);
        }


        public static AuthorizationContext CreateAuthorizationContext(ClaimsPrincipal principal, string action, params string[] resources)
        {
            var actionClaims = new Collection<Claim>
            {
                new Claim(ActionType, action)
            };

            var resourceClaims = new Collection<Claim>();

            if (resources != null && resources.Length > 0)
            {
                resources.ToList().ForEach(ar => resourceClaims.Add(new Claim(ResourceType, ar)));
            }

            return new AuthorizationContext(
                principal,
                resourceClaims,
                actionClaims);
        }
    }
}
