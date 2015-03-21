namespace SRP_DI_Workshop.Domain.Revenue
{
    public interface IPricingCalculationFactory
    {
        IPricingCalculation GetPricingCalculation(string calculationId);
    }
}
