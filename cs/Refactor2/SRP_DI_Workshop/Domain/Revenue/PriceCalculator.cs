﻿using System;

namespace SRP_DI_Workshop.Domain.Revenue
{
    public class PriceCalculator : IPriceCalculator
    {
        private readonly IDependencyResolver _dependencyResolver;

        public PriceCalculator(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public void CalculatePrice(OrderItem item, InventoryItem inventoryItem)
        {
            IPricingCalculation pricingCalculation;

            switch (inventoryItem.PricingCalculation)
            {
                case FullQuantityPricingCalculation.Id:
                    pricingCalculation = _dependencyResolver.Resolve<FullQuantityPricingCalculation>();
                    break;
                case Buy2Get1FreePricingCalculation.Id:
                    pricingCalculation = _dependencyResolver.Resolve<Buy2Get1FreePricingCalculation>();
                    break;
                default:
                    throw new InvalidOperationException("Invalid pricing calculation type: " + inventoryItem.PricingCalculation);
            }

            pricingCalculation.CalculatePrice(item, inventoryItem);
        }
    }
}