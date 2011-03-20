using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class UnityRegistry : IUnityRegistry
    {
        private readonly List<Expression> configurations = new List<Expression>();
        private readonly List<Expression> extensions = new List<Expression>();
        private readonly List<Expression> registrations = new List<Expression>();
        private readonly List<UnityRegistry> registries = new List<UnityRegistry>();

        public void Scan(Action<IAssemblyScanner> action)
        {
            var assemblyScanner = new AssemblyScanner();

            action(assemblyScanner);

            assemblyScanner.Scan(this);
        }

        public void AddRegistry<T>() where T : UnityRegistry, new()
        {
            AddRegistry(new T());
        }

        public void AddRegistry(UnityRegistry registry)
        {
            registries.Add(registry);
        }

        public RegistrationExpression Register(Type typeFrom, Type typeTo)
        {
            var registrationExpression = new RegistrationExpression(typeFrom, typeTo);
            registrations.Add(registrationExpression);
            return registrationExpression;
        }

        public RegistrationExpression Register(Type type)
        {
            return Register(null, type);
        }

        public RegistrationExpression Register<TFrom, TTo>() where TTo : TFrom
        {
            return Register(typeof (TFrom), typeof (TTo));
        }

        public RegistrationExpression Register<T>()
        {
            return Register(typeof (T));
        }

        public FactoryRegistrationExpression<TFrom> Register<TFrom>(Func<IUnityContainer, TFrom> factoryDelegate)
        {
            var factoryRegistrationExpression = new FactoryRegistrationExpression<TFrom>(factoryDelegate);
            registrations.Add(factoryRegistrationExpression);

            return factoryRegistrationExpression;
        }

        [Obsolete("Use Register<T>().AsSingleton() instead.")]
        public void MakeSingleton<T>()
        {
            MakeSingleton<T>(null);
        }

        [Obsolete("Use Register<T>().WithName(name).AsSingleton() instead.")]
        public void MakeSingleton<T>(string namedInstance)
        {
            var registrationExpression = new RegistrationExpression(null, typeof(T), namedInstance, () => new ContainerControlledLifetimeManager());
            registrations.Add(registrationExpression);
        }

        public ConfigureTypeExpression<T> ConfigureCtorArgsFor<T>(params object[] args)
        {
            var configurationExpression = new ConfigureTypeExpression<T>(new InjectionConstructor(args));
            configurations.Add(configurationExpression);

            return configurationExpression;
        }

        public ConfigureTypeExpression<T> SelectConstructor<T>(params Type[] args)
        {
            return ConfigureCtorArgsFor<T>(args);
        }

        public AddNewExtensionExpression<T> AddExtension<T>() where T : UnityContainerExtension, new()
        {
            var extensionExpression = new AddNewExtensionExpression<T>();
            extensions.Add(extensionExpression);
            return extensionExpression;
        }

        public ConfigureExtensionExpression<T> ConfigureExtension<T>(Action<T> configAction) where T : IUnityContainerExtensionConfigurator
        {
            var configureExtensionExpression = new ConfigureExtensionExpression<T>(configAction);
            configurations.Add(configureExtensionExpression);

            return configureExtensionExpression;
        }

        public PostBuildUpExpression<T> AfterBuildingUp<T>() where T : class
        {
            var afterBuildUpExpression = new PostBuildUpExpression<T>();
            extensions.Add(afterBuildUpExpression);
            return afterBuildUpExpression;
        }

        public virtual void Configure(IUnityContainer container)
        {
            registries.ForEach(x => x.Configure(container));
            extensions.ForEach(expression => expression.Execute(container));
            registrations.ForEach(expression => expression.Execute(container));
            configurations.ForEach(expression => expression.Execute(container));
        }
    }
}