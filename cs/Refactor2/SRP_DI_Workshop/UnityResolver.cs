using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using SRP_DI_Workshop.Domain.OrderOperations;
using SRP_DI_Workshop.Domain.Revenue;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public class UnityResolver : IDependencyResolver
    {
        private readonly UnityContainer _container = new UnityContainer();

        public UnityResolver()
        {
            ConfigureContainer(_container);
        }

        private void ConfigureContainer(UnityContainer container)
        {
            // transients
            _container
                .RegisterType<ILoggerService, LoggerService>()
                .RegisterType<IPriceCalculator, PriceCalculator>()
                .RegisterType<IOrderOperation, CreateOrderOperation>(typeof(CreateOrderOperation).FullName)
                .RegisterType<IOrderOperation, CancelOrderOperation>(typeof(CancelOrderOperation).FullName)
                .RegisterType<IOrderOperation, GetOrderOperation>(typeof(GetOrderOperation).FullName)
                .RegisterType<IEnumerable<IOrderOperation>, IOrderOperation[]>();

            // singletons
            _container
                .RegisterType<OrderProcessor>(new ContainerControlledLifetimeManager())
                .RegisterType<IResponseMessageFactory, ResponseMessageFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IDeduplicator, Deduplicator>(new ContainerControlledLifetimeManager())
                .RegisterType<IOrderRepository, OrderRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IInventoryRepository, InventoryRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IPricingCalculationFactory, PricingCalculationFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IOrderOperationFactory, OrderOperationFactory>(new ContainerControlledLifetimeManager());

            _container
                .RegisterInstance(typeof (IDependencyResolver), this);
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }
    }
}
