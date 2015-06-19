using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bootcamp.UI.Features.Lookups;
using MediatR;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Bootcamp.UI.DependencyResolution.Registries
{
    public class MediatorRegistry : Registry
    {
        public MediatorRegistry()
        {
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<MediatorRegistry>(); // Our assembly with requests & handlers
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
            });

           
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();

        }
    }
}