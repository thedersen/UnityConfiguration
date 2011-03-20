using System;

namespace UnityConfiguration
{
    /// <summary>
    /// Convention for registering several types implementing the same interface.
    /// By default it is registered with the name of the type, but this can be overridden.
    /// </summary>
    public class AddAllConvention : IAssemblyScannerConvention, ILifetimePolicyExpression
    {
        private Func<Type, string> getName = t => t.Name;
        private Type interfaceType;
        private Action<ILifetimePolicyExpression> lifetimePolicyAction = x => x.AsTransient();

        /// <summary>
        /// Specify the type to register multiple instances of.
        /// </summary>
        /// <typeparam name="T">The type to register multiple instances of.</typeparam>
        /// <returns>
        /// An instance of the <see cref="AddAllConvention"/> that can be used to
        /// further configure the convention.
        /// </returns>
        public AddAllConvention TypesImplementing<T>()
        {
            interfaceType = typeof (T);
            return this;
        }

        /// <summary>
        /// Specify how to resolve the name for the registration.
        /// </summary>
        /// <param name="func">The function to create the name for the specified type.</param>
        /// <returns>
        /// An instance of the <see cref="AddAllConvention"/> that can be used to
        /// further configure the convention.
        /// </returns>
        public AddAllConvention WithName(Func<Type, string> func)
        {
            getName = func;
            return this;
        }

        /// <summary>
        /// Indicates that only a single instance of the binding should be created, and then
        /// should be re-used for all subsequent requests.
        /// </summary>
        public void AsSingleton()
        {
            lifetimePolicyAction = x => x.AsSingleton();
        }

        /// <summary>
        /// Indicates that instances activated via the binding should not be re-used, nor have
        /// their lifecycle managed by Ninject.
        /// </summary>
        public void AsTransient()
        {
            lifetimePolicyAction = x => x.AsTransient();
        }

        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same thread.
        /// </summary>
        public void AsPerThread()
        {
            lifetimePolicyAction = x => x.AsPerThread();
        }

        void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
        {
            if (type.CanBeCastTo(interfaceType) && type.CanBeCreated())
            {
                var expression = registry.Register(interfaceType, type).WithName(getName(type));

                lifetimePolicyAction(expression);
            }
        }
    }
}