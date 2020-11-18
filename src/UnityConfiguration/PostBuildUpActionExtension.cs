using System;
using Unity;
using Unity.Builder;
using Unity.Extension;
using Unity.Strategies;

namespace UnityConfiguration
{
    public class PostBuildUpActionExtension<T> : UnityContainerExtension where T : class
    {
        private readonly Action<IUnityContainer, T> action;

        public PostBuildUpActionExtension(Action<IUnityContainer, T> action)
        {
            this.action = action;
        }

        protected override void Initialize()
        {
            Context.Strategies.Add(new PostBuildUpActionStrategy<T>(action, Container),
                                   UnityBuildStage.PostInitialization);
        }
    }

    public class PostBuildUpActionStrategy<T> : BuilderStrategy where T : class
    {
        private readonly Action<IUnityContainer, T> action;
        private readonly IUnityContainer container;

        public PostBuildUpActionStrategy(Action<IUnityContainer, T> action, IUnityContainer container)
        {
            this.action = action;
            this.container = container;
        }

        public override void PostBuildUp(ref BuilderContext context)
        {
            if (context.Existing is T obj)
                action(container, obj);
        }
    }
}