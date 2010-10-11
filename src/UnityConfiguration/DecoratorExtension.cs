using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

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
        private readonly Func<IUnityContainer, T, object> func;
        private readonly IUnityContainer container;

        public DecoratorStrategy(Func<IUnityContainer, T, object> func, IUnityContainer container)
        {
            this.func = func;
            this.container = container;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            var obj = context.Existing as T;

            if (obj != null)
                context.Existing = func(container, obj);
        }
    }
}