namespace SRP_DI_Workshop
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
    }
}