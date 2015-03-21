namespace SRP_DI_Workshop.Domain
{
    public enum OrderState
    {
        NotProcessed = 0,
        Filled,
        Cancelled,
        Processing,
        Shipped,
    }
}