using System;
using System.Linq;
using Unity.Lifetime;

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
        private Action<ILifetimePolicyExpression> lifetimePolicyAction;

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
        /// Specify the type to register multiple instances of.
        /// </summary>
        /// <param name="type">The type to register multiple instances of.</param>
        /// <returns>
        /// An instance of the <see cref="AddAllConvention"/> that can be used to
        /// further configure the convention.
        /// </returns>
        public AddAllConvention TypesImplementing(Type type)
        {
            interfaceType = type;
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

        public void Using<T>() where T : LifetimeManager, new()
        {
            lifetimePolicyAction = x => x.Using<T>();
        }

        void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
        {
            Type typeFrom = null;
            if (type.CanBeCastTo(interfaceType))
            {
                typeFrom = interfaceType;
            }
            else if(type.ImplementsInterfaceTemplate(interfaceType))
            {

                typeFrom = type.GetInterfaces().FirstOrDefault(i => i.GetGenericTypeDefinition() == interfaceType);
            }

            if (typeFrom != null && type.CanBeCreated())
            {
                var expression = registry.Register(typeFrom, type).WithName(getName(type));

                if (lifetimePolicyAction != null)
                    lifetimePolicyAction(expression);
            }
        }
    }
}