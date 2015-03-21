using System;
using System.Collections.Generic;

namespace SRP_DI_Workshop.Domain.Revenue
{
    public class PricingCalculationFactory : IPricingCalculationFactory
    {
        private readonly IDependencyResolver _dependencyResolver;
        private readonly Dictionary<string, Type> _calculationtypes;

        public PricingCalculationFactory(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;

            _calculationtypes = new Dictionary<string, Type>
            {
                { Buy2Get1FreePricingCalculation.Id, typeof(Buy2Get1FreePricingCalculation)},
                { FullQuantityPricingCalculation.Id, typeof(FullQuantityPricingCalculation)}
            };
        }

        public IPricingCalculation GetPricingCalculation(string calculationId)
        {
            Type calcType;

            if (_calculationtypes.TryGetValue(calculationId, out calcType))
                return _dependencyResolver.Resolve(calcType) as IPricingCalculation;

            throw new InvalidOperationException("Invalid pricing calculation type: " + calculationId);
        }
    }
}
