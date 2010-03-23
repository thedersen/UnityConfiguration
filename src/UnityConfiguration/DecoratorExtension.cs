using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace UnityConfiguration
{
    public class DecoratorExtension<T> : UnityContainerExtension where T : class
    {
        private readonly Func<T, object> func;

        public DecoratorExtension(Func<T, object> func)
        {
            this.func = func;
        }

        protected override void Initialize()
        {
            Context.Strategies.Add(new DecoratorStrategy<T>(func), UnityBuildStage.PostInitialization);
        }
    }

    public class DecoratorStrategy<T> : BuilderStrategy where T : class
    {
        private readonly Func<T, object> func;

        public DecoratorStrategy(Func<T, object> func)
        {
            this.func = func;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            var obj = context.Existing as T;

            if (obj != null)
                context.Existing = func(obj);
        }
    }
}