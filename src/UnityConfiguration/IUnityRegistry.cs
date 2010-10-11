using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public interface IUnityRegistry
    {
        void Scan(Action<IAssemblyScanner> action);
        RegistrationExpression Register(Type typeFrom, Type typeTo);
        RegistrationExpression Register<TFrom, TTo>() where TTo : TFrom;
        FactoryRegistrationExpression<TFrom> Register<TFrom>(Func<IUnityContainer, object> factoryDelegate);
        LifetimeExpression MakeSingleton<T>();
        LifetimeExpression MakeSingleton<T>(string namedInstance);
        ConfigurationExpression<T> ConfigureCtorArgsFor<T>(params object[] args);
        ConfigurationExpression<T> SelectConstructor<T>(params Type[] args);
        ExtensionExpression<T> AddExtension<T>() where T : UnityContainerExtension, new();
        PostBuildUpExpression<T> AfterBuildingUp<T>() where T : class;
    }
}
