using System;
using System.ComponentModel;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public interface IUnityRegistry : IHideObjectMembers
    {
        /// <summary>
        /// Scan a set of assemblies.
        /// </summary>
        /// <param name="action">A nested closure for configuring the <see cref="IAssemblyScanner"/>.</param>
        void Scan(Action<IAssemblyScanner> action);

        /// <summary>
        /// Import a <see cref="UnityRegistry"/> into this.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="UnityRegistry"/> to import.</typeparam>
        void AddRegistry<T>() where T : UnityRegistry, new();

        /// <summary>
        /// Import a <see cref="UnityRegistry"/> into this.
        /// </summary>
        /// <param name="registry">An instance of the <see cref="UnityRegistry"/> to import.</param>
        void AddRegistry(UnityRegistry registry);

        /// <summary>
        /// Register a type mapping in the container by using a concrete instance.
        /// </summary>
        /// <param name="typeFrom">The type that will be requested.</param>
        /// <param name="instanceTo">The instance that will be returned.</param>
        /// <returns>
        /// An instance of a <see cref="RegistrationExpression"/> that can be used to 
        /// further configure the registration.
        /// </returns>
        RegistrationExpression Register(Type typeFrom,object instanceTo);

        /// <summary>
        /// Register a type mapping in the container by using a concrete instance.
        /// </summary>
        /// <typeparam name="tFrom">The type that will be requested.</typeparam>
        /// <param name="instanceTo">The instance that will be returned.</param>
        /// <returns>
        /// An instance of a <see cref="RegistrationExpression"/> that can be used to 
        /// further configure the registration.
        /// </returns>
        RegistrationExpression Register<tFrom>(object instanceTo) ;

        /// <summary>
        /// Register a type mapping in the container.
        /// </summary>
        /// <param name="typeFrom">The type that will be requested.</param>
        /// <param name="typeTo">The type that will actually be returned.</param>
        /// <returns>
        /// An instance of a <see cref="RegistrationExpression"/> that can be used to 
        /// further configure the registration.
        /// </returns>
        RegistrationExpression Register(Type typeFrom, Type typeTo);

        /// <summary>
        /// Register a type mapping in the container.
        /// </summary>
        /// <typeparam name="TFrom">The type that will be requested.</typeparam>
        /// <typeparam name="TTo">The type that will actually be returned.</typeparam>
        /// <returns>
        /// An instance of a <see cref="RegistrationExpression"/> that can be used to 
        /// further configure the registration.
        /// </returns>
        RegistrationExpression Register<TFrom, TTo>() where TTo : TFrom;

        /// <summary>
        /// Register a type mapping in the container by using a factory delegate.
        /// </summary>
        /// <typeparam name="TFrom">The type that will be requested.</typeparam>
        /// <param name="factoryDelegate">The factory delegate that will be used to 
        /// construct the type that will actually be returned.</param>
        /// <returns>
        /// An instance of a <see cref="FactoryRegistrationExpression{TFrom}"/> that can be used to 
        /// further configure the registration.
        /// </returns>
        FactoryRegistrationExpression<TFrom> Register<TFrom>(Func<IUnityContainer, TFrom> factoryDelegate);

        /// <summary>
        /// Configure a type mapping in the container.
        /// </summary>
        /// <param name="type">The type to configure. Can be both the requested or the returned type.</param>
        /// <returns>
        /// An instance of a <see cref="RegistrationExpression"/> that can be used to 
        /// further configure the registration.
        /// </returns>
        RegistrationExpression Configure(Type type);

        /// <summary>
        /// Configure a type mapping in the container.
        /// </summary>
        /// <typeparam name="T">The type to configure. Can be both the requested or the returned type.</typeparam>
        /// <returns>
        /// An instance of a <see cref="RegistrationExpression"/> that can be used to 
        /// further configure the registration.
        /// </returns>
        RegistrationExpression Configure<T>();

        /// <summary>
        /// A shortcut method to make a registered type a singleton. Mostly useful for making types registered 
        /// by a convention.
        /// </summary>
        /// <typeparam name="T">The type to make singleton. Can be both an interface or a concrete type.</typeparam>
        [Obsolete("Use Configure<T>().AsSingleton() instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void MakeSingleton<T>();

        /// <summary>
        /// A shortcut method to make a registered type a singleton. Mostly useful for making types registered 
        /// by a convention.
        /// </summary>
        /// <typeparam name="T">The type to make singleton. Can be both an interface or a concrete type.</typeparam>
        /// <param name="namedInstance">Name of the instance.</param>
        [Obsolete("Use Configure<T>().WithName(name).AsSingleton() instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void MakeSingleton<T>(string namedInstance);

        /// <summary>
        /// Specify parameters that will be passed to the constructor when constructing the type.
        /// If some of the parameters should be resolved from the container, specify its type.
        /// </summary>
        /// <typeparam name="T">The type to configure.</typeparam>
        /// <param name="args">Value or type of the parameters.</param>
        [Obsolete("Use Configure<T>().WithConstructorArguments(params object[])")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void ConfigureCtorArgsFor<T>(params object[] args);

        /// <summary>
        /// Select the constructor to be used when constructing the type by specifying 
        /// the types of the parameters in the constructor to use.
        /// </summary>
        /// <typeparam name="T">The type to configure.</typeparam>
        /// <param name="args">The types of the parameters or empty to specify default constructor.</param>
        void SelectConstructor<T>(params Type[] args);

        /// <summary>
        /// Add a <see cref="UnityContainerExtension"/> to the container.
        /// </summary>
        /// <typeparam name="T">Type of the extension to add.</typeparam>
        /// <returns></returns>
        AddNewExtensionExpression<T> AddExtension<T>() where T : UnityContainerExtension, new();

        /// <summary>
        /// Confgure an extension already registered in the container.
        /// </summary>
        /// <typeparam name="T">Type of the extension to configure.</typeparam>
        /// <param name="configAction">A nested closure that can be used to configure the extension.</param>
        /// <returns></returns>
        ConfigureExtensionExpression<T> ConfigureExtension<T>(Action<T> configAction) where T : IUnityContainerExtensionConfigurator;

        /// <summary>
        /// Allows for some actions to be applied to a type after it is constructed.
        /// </summary>
        /// <typeparam name="T">The type to apply actions on.</typeparam>
        /// <returns>
        /// An instance of a <see cref="PostBuildUpExpression{T}"/> that holds the actions
        /// that can be applied.
        /// </returns>
        PostBuildUpExpression<T> AfterBuildingUp<T>() where T : class;
    }
}