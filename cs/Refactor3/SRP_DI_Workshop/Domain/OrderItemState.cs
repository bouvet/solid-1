namespace SRP_DI_Workshop.Domain
{
    public enum OrderItemState
    {
        NotProcessed = 0,
        Filled,
        Cancelled,
        NotEnoughQuantityOnHand
    }
}