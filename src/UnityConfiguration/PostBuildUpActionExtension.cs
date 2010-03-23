using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace UnityConfiguration
{
    public class PostBuildUpActionExtension<T> : UnityContainerExtension where T : class
    {
        private readonly Action<T> action;

        public PostBuildUpActionExtension(Action<T> action)
        {
            this.action = action;
        }

        protected override void Initialize()
        {
            Context.Strategies.Add(new PostBuildUpActionStrategy<T>(action), UnityBuildStage.PostInitialization);
        }
    }

    public class PostBuildUpActionStrategy<T> : BuilderStrategy where T : class
    {
        private readonly Action<T> action;

        public PostBuildUpActionStrategy(Action<T> action)
        {
            this.action = action;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            var obj = context.Existing as T;

            if (obj != null)
                action(obj);
        }
    }
}