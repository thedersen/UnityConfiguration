using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class UnityRegistry : IUnityRegistry
    {
        private readonly List<Expression> extensions = new List<Expression>();
        private readonly List<Expression> registrations = new List<Expression>();
        private readonly List<Expression> configurations = new List<Expression>();

        public void Configure(IUnityContainer container)
        {
            extensions.ForEach(expression => expression.Execute(container));
            registrations.ForEach(expression => expression.Execute(container));
            configurations.ForEach(expression => expression.Execute(container));
        }

        public void Scan(Action<IAssemblyScanner> action)
        {
            var assemblyScanner = new AssemblyScanner();

            action(assemblyScanner);

            assemblyScanner.Scan(this);
        }

        public RegistrationExpression Register(Type typeFrom, Type typeTo)
        {
            var registrationExpression = new RegistrationExpression(typeFrom,typeTo);
            registrations.Add(registrationExpression);
            return registrationExpression;
        }

        public RegistrationExpression Register<TFrom, TTo>() where TTo : TFrom
        {
            return Register(typeof (TFrom), typeof (TTo));
        }

        public FactoryRegistrationExpression<TFrom> Register<TFrom>(Func<IUnityContainer, object> factoryDelegate)
        {
            var factoryRegistrationExpression = new FactoryRegistrationExpression<TFrom>(factoryDelegate);
            registrations.Add(factoryRegistrationExpression);

            return factoryRegistrationExpression;
        }

        public LifetimeExpression<T> MakeSingleton<T>()
        {
            var lifetimeExpression = new LifetimeExpression<T>(new ContainerControlledLifetimeManager());
            configurations.Add(lifetimeExpression);

            return lifetimeExpression;
        }

        public ConfigurationExpression<T> ConfigureCtorArgsFor<T>(params object[] args)
        {
            var configurationExpression = new ConfigurationExpression<T>(new InjectionConstructor(args));
            configurations.Add(configurationExpression);

            return configurationExpression;
        }

        public ConfigurationExpression<T> SelectConstructor<T>(params Type[] args)
        {
            return ConfigureCtorArgsFor<T>(args);
        }

        public ExtensionExpression AddExtension<T>() where T : UnityContainerExtension, new()
        {
            var extensionExpression = new ExtensionExpression(new T());
            extensions.Add(extensionExpression);
            return extensionExpression;
        }

        public void AfterBuildUp<T>(Action<T> action) where T : class
        {
            var extensionExpression = new ExtensionExpression(new AfterBuildUpExtension<T>(action));
            extensions.Add(extensionExpression);
        }
    }
}