using System;
using Unity;
using Unity.Builder;
using Unity.Extension;
using Unity.Strategies;

namespace UnityConfiguration
{
    public class DecoratorExtension<T> : UnityContainerExtension where T : class
    {
        private readonly Func<IUnityContainer, T, object> func;

        public DecoratorExtension(Func<IUnityContainer, T, object> func)
        {
            this.func = func;
        }

        protected override void Initialize()
        {
            Context.Strategies.Add(new DecoratorStrategy<T>(func, Container), UnityBuildStage.PostInitialization);
        }
    }

    public class DecoratorStrategy<T> : BuilderStrategy where T : class
    {
        private readonly IUnityContainer container;
        private readonly Func<IUnityContainer, T, object> func;

        public DecoratorStrategy(Func<IUnityContainer, T, object> func, IUnityContainer container)
        {
            this.func = func;
            this.container = container;
        }

        public override void PostBuildUp(ref BuilderContext context)
        {
            if (context.Existing is T obj)
                context.Existing = func(container, obj);
        }
    }
}