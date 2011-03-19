using System;

namespace UnityConfiguration
{
    /// <summary>
    /// Convention for registering several types implementing the same interface.
    /// By default it is registered with the name of the type, but this can be overridden.
    /// </summary>
    public class AddAllConvention : IAssemblyScannerConvention, ILifetimePolicyExpression
    {
        private bool asSingleton;
        private Func<Type, string> getName = t => t.Name;
        private Type interfaceType;

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
        /// Register all types found by this convention as a singleton instance.
        /// </summary>
        public void AsSingleton()
        {
            asSingleton = true;
        }

        void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
        {
            if (type.CanBeCastTo(interfaceType) && type.CanBeCreated())
            {
                if (asSingleton)
                {
                    registry.Register(interfaceType, type).WithName(getName(type)).AsSingleton();
                }
                else
                {
                    registry.Register(interfaceType, type).WithName(getName(type));
                }
            }
        }
    }
}