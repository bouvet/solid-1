using System;
using System.Collections.Generic;
using SRP_DI_Workshop.Domain.OrderOperations;
using SRP_DI_Workshop.Domain.Revenue;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<Type, Func<object>> _objectFactories;

        private readonly ResponseMessageFactory _responseMessageFactoryInstance;
        private readonly Deduplicator _deduplicatorInstance;
        private readonly OrderProcessor _orderProcessorInstance;
        private readonly OrderRepository _orderRepositoryInstance;
        private readonly Inventoryrepository _inventoryServiceInstance;

        public DependencyResolver()
        {
            _objectFactories = new Dictionary<Type, Func<object>>
            {
                {typeof(IResponseMessageFactory), ResolveResponseMessageFactory},
                {typeof(IDeduplicator), ResolveDeduplicator},
                {typeof(ILoggerService), ResolveLoggerService},
                {typeof(IOrderRepository), ResolveOrderRepository},
                {typeof(IInventoryRepository), ResolveInventoryService},
                {typeof(OrderProcessor), ResolveOrderProcessor},
                {typeof(CancelOrderOperation), ResolveCancelOrderOperation},
                {typeof(GetOrderOperation), ResolveGetOrderOperation},
                {typeof(CreateOrderOperation), ResolveCreateOrderOperation},
                {typeof(Buy2Get1FreePricingCalculation), ResolveBuy2Get1FreePricingCalculation},
                {typeof(FullQuantityPricingCalculation), ResolveFullQuantityPricingCalculation},
                {typeof(IPriceCalculator), ResolvePriceCalculator}
            };

            _responseMessageFactoryInstance = new ResponseMessageFactory();
            _deduplicatorInstance = new Deduplicator();
            _orderProcessorInstance = new OrderProcessor(Resolve<IResponseMessageFactory>(), Resolve<ILoggerService>(), Resolve<IDeduplicator>(), this);
            _orderRepositoryInstance = new OrderRepository(Resolve<ILoggerService>());
            _inventoryServiceInstance = new Inventoryrepository(Resolve<ILoggerService>());
        }

        public T Resolve<T>()
        {
            Func<object> objectFactory;

            if (_objectFactories.TryGetValue(typeof (T), out objectFactory)
                && objectFactory != null)
            {
                object resolvedObject = objectFactory();

                if (resolvedObject != null)
                    return (T) resolvedObject;
            }
            
            throw new ArgumentOutOfRangeException(string.Format("Failed to resolve type [{0}]", typeof(T)));
        }

        private OrderProcessor ResolveOrderProcessor()
        {
            return _orderProcessorInstance;
        }

        private IResponseMessageFactory ResolveResponseMessageFactory()
        {
            return _responseMessageFactoryInstance;
        }

        private IDeduplicator ResolveDeduplicator()
        {
            return _deduplicatorInstance;
        }

        private ILoggerService ResolveLoggerService()
        {
            return new LoggerService();
        }

        private IInventoryRepository ResolveInventoryService()
        {
            return _inventoryServiceInstance;
        }

        private IOrderRepository ResolveOrderRepository()
        {
            return _orderRepositoryInstance;
        }

        private IPriceCalculator ResolvePriceCalculator()
        {
            return new PriceCalculator(this);
        }

        private FullQuantityPricingCalculation ResolveFullQuantityPricingCalculation()
        {
            return new FullQuantityPricingCalculation();
        }

        private Buy2Get1FreePricingCalculation ResolveBuy2Get1FreePricingCalculation()
        {
            return new Buy2Get1FreePricingCalculation();
        }

        private GetOrderOperation ResolveGetOrderOperation()
        {
            return new GetOrderOperation(Resolve<IOrderRepository>(), Resolve<ILoggerService>(), Resolve<IResponseMessageFactory>());
        }

        private CancelOrderOperation ResolveCancelOrderOperation()
        {
            return new CancelOrderOperation(Resolve<IResponseMessageFactory>(), Resolve<ILoggerService>(), Resolve<IOrderRepository>(), Resolve<IInventoryRepository>());
        }

        private CreateOrderOperation ResolveCreateOrderOperation()
        {
            return new CreateOrderOperation(Resolve<IPriceCalculator>(), Resolve<IInventoryRepository>(), Resolve<IOrderRepository>(), Resolve<ILoggerService>(), Resolve<IResponseMessageFactory>());
        }
    }
}
