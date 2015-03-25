using System;
using System.Collections.Generic;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly Dictionary<Type, Func<object>> _objectFactories;

        private readonly IResponseMessageFactory _responseMessageFactoryInstance;

        public DependencyResolver()
        {
            _objectFactories = new Dictionary<Type, Func<object>>
            {
                {typeof(IResponseMessageFactory), ResolveResponseMessageFactory},
            };

            _responseMessageFactoryInstance = new ResponseMessageFactory();
        }

        public T Resolve<T>()
        {
            return (T) Resolve(typeof (T));
        }

        public object Resolve(Type type)
        {
            Func<object> objectFactory;

            if (_objectFactories.TryGetValue(type, out objectFactory)
                && objectFactory != null)
            {
                object resolvedObject = objectFactory();

                if (resolvedObject != null)
                    return resolvedObject;
            }

            throw new ArgumentOutOfRangeException(string.Format("Failed to resolve type [{0}]", type));
        }

        private IResponseMessageFactory ResolveResponseMessageFactory()
        {
            return _responseMessageFactoryInstance;
        }
    }
}
