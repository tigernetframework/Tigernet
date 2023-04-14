using Tigernet.Hosting.DataAccess.Brokers;
using Tigernet.Hosting.DataAccess.Services;

namespace Tigernet.Hosting
{
    public partial class TigernetHostBuilder
    {
        /// <summary>
        /// DI Container 
        /// </summary>
        private readonly Dictionary<Type, Type> _services;

        /// <summary>
        /// Adds interface and implementation of it to DI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        public void AddService<T, TImpl>() where TImpl : T
        {
            _services[typeof(T)] = typeof(TImpl);
        }

        public void AddDataSourceProvider<TInterface, TImplementation>() where TImplementation : class, IDataSourceBroker, new()
        {
            _services[typeof(TInterface)] = typeof(TImplementation);

            // TODO : Optimize
            // Registering entity manager services
            var entityTypes = new TImplementation().GetEntityTypes();

            var serviceType = typeof(IEntityManagerBaseService<>);
            var targetType = typeof(EntityManagerBaseService<>);

            foreach (var entityType in entityTypes)
            {
                var specificInterface = serviceType.MakeGenericType(entityType);
                var specificImplementation = targetType.MakeGenericType(entityType);
                _services[specificInterface] = specificImplementation;
            }
        }

        /// <summary>
        /// Get added service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            return (T)GetService(typeof(T));
        }

        /// <summary>
        /// Gets added service and constructs it returning as object
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private object GetService(Type type)
        {
            if (_services.TryGetValue(type, out var implementationType))
            {
                var constructor = implementationType.GetConstructors()[0];
                var parameters = constructor.GetParameters();

                if (parameters.Length == 0)
                {
                    return Activator.CreateInstance(implementationType);
                }

                var parameterInstances = new object[parameters.Length];

                for (var i = 0; i < parameters.Length; i++)
                {
                    parameterInstances[i] = GetService(parameters[i].ParameterType);
                }

                return constructor.Invoke(parameterInstances);
            }

            throw new InvalidOperationException($"Type {type} not registered");
        }
    }
}