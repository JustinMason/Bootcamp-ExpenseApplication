using System;
using System.Web;
using System.Web.Mvc;

namespace Bootcamp.UI.DependencyResolution
{
    /// <summary>
    /// http://haacked.com/archive/2011/06/03/dependency-injection-with-asp-net-httpmodules.aspx/
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    public class ContainerHttpModule<TModule>: IHttpModule 
        where TModule : IHttpModule
    {
        readonly Lazy<IHttpModule> _module = new Lazy<IHttpModule>(RetrieveModule);

        private static IHttpModule RetrieveModule()
        {
            return DependencyResolver.Current.GetService<TModule>();
        }

        public void Dispose()
        {
            _module.Value.Dispose();
        }

        public void Init(HttpApplication context)
        {
            _module.Value.Init(context);
        }
    }
}