using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public interface IUnityRegistry
    {
        void Scan(Action<IAssemblyScanner> action);
        void AddRegistry<T>() where T : UnityRegistry, new();
        void AddRegistry(UnityRegistry registry);
        RegistrationExpression Register(Type typeFrom, Type typeTo);
        RegistrationExpression Register<TFrom, TTo>() where TTo : TFrom;
        FactoryRegistrationExpression<TFrom> Register<TFrom>(Func<IUnityContainer, TFrom> factoryDelegate);
        LifetimeExpression MakeSingleton<T>();
        LifetimeExpression MakeSingleton<T>(string namedInstance);
        ConfigureTypeExpression<T> ConfigureCtorArgsFor<T>(params object[] args);
        ConfigureTypeExpression<T> SelectConstructor<T>(params Type[] args);
        AddNewExtensionExpression<T> AddExtension<T>() where T : UnityContainerExtension, new();
        ConfigureExtensionExpression<T> ConfigureExtension<T>(Action<T> configAction) where T : IUnityContainerExtensionConfigurator;
        PostBuildUpExpression<T> AfterBuildingUp<T>() where T : class;
    }
}