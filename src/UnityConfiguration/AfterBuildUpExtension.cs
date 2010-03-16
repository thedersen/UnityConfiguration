using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace UnityConfiguration
{
    public class AfterBuildUpExtension<T> : UnityContainerExtension where T : class
    {
        private readonly Action<T> action;

        public AfterBuildUpExtension(Action<T> action)
        {
            this.action = action;
        }

        protected override void Initialize()
        {
            Context.Strategies.Add(new AfterBuildUpStrategy<T>(action), UnityBuildStage.PostInitialization);
        }
    }

    public class AfterBuildUpStrategy<T> : BuilderStrategy where T : class
    {
        private readonly Action<T> action;

        public AfterBuildUpStrategy(Action<T> action)
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