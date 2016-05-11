using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
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
            assemblyScanner.Exclude(t => t == GetType());
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

        public RegistrationExpression Register(Type typeFrom, object instanceTo)
        {
            var registrationExpression = new InstanceBasedRegistrationExpression(typeFrom, instanceTo);
            registrations.Add(registrationExpression);
            return registrationExpression;
        }

        public RegistrationExpression Register<tFrom>(object instanceTo)
        {
            return Register(typeof(tFrom),instanceTo);
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

        public RegistrationExpression Configure(Type type)
        {
            return Register(null, type);
        }

        public RegistrationExpression Configure<T>()
        {
            return Configure(typeof (T));
        }

        [Obsolete("Use Configure<T>().AsSingleton() instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void MakeSingleton<T>()
        {
            Configure<T>().AsSingleton();
        }

        [Obsolete("Use Configure<T>().WithName(name).AsSingleton() instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void MakeSingleton<T>(string namedInstance)
        {
            Configure<T>().WithName(namedInstance).AsSingleton();
        }

        [Obsolete("Use Configure<T>().WithConstructorArguments(params object[])")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ConfigureCtorArgsFor<T>(params object[] args)
        {
            Configure<T>().WithConstructorArguments(args);
        }

        public void SelectConstructor<T>(params Type[] args)
        {
            Configure<T>().WithConstructorArguments(args);
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