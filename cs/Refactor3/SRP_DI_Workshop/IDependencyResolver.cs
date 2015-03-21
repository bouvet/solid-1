using System;

namespace SRP_DI_Workshop
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
        object Resolve(Type type);
    }
}