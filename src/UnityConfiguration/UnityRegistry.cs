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

        public void Scan(Action<IAssemblyScanner> action)
        {
            var assemblyScanner = new AssemblyScanner();

            action(assemblyScanner);

            assemblyScanner.Scan(this);
        }

        public RegistrationExpression Register(Type typeFrom, Type typeTo)
        {
            var registrationExpression = new RegistrationExpression(typeFrom, typeTo);
            registrations.Add(registrationExpression);
            return registrationExpression;
        }

        public RegistrationExpression Register<TFrom, TTo>() where TTo : TFrom
        {
            return Register(typeof (TFrom), typeof (TTo));
        }

        public FactoryRegistrationExpression<TFrom> Register<TFrom>(Func<IUnityContainer, TFrom> factoryDelegate)
        {
            var factoryRegistrationExpression = new FactoryRegistrationExpression<TFrom>(factoryDelegate);
            registrations.Add(factoryRegistrationExpression);

            return factoryRegistrationExpression;
        }

        public LifetimeExpression MakeSingleton<T>()
        {
            return MakeSingleton<T>(null);
        }

        public LifetimeExpression MakeSingleton<T>(string namedInstance)
        {
            var lifetimeExpression = new LifetimeExpression(typeof (T), () => new ContainerControlledLifetimeManager(),
                                                            namedInstance);
            configurations.Add(lifetimeExpression);

            return lifetimeExpression;
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

        public PostBuildUpExpression<T> AfterBuildingUp<T>() where T : class
        {
            var afterBuildUpExpression = new PostBuildUpExpression<T>();
            extensions.Add(afterBuildUpExpression);
            return afterBuildUpExpression;
        }

        public virtual void Configure(IUnityContainer container)
        {
            extensions.ForEach(expression => expression.Execute(container));
            registrations.ForEach(expression => expression.Execute(container));
            configurations.ForEach(expression => expression.Execute(container));
        }
    }
}