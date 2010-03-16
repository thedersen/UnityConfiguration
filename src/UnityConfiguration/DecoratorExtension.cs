using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace UnityConfiguration
{
    public class DecoratorExtension<T> : UnityContainerExtension
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

    public class DecoratorStrategy<T> : BuilderStrategy
    {
        private readonly Func<T, object> func;

        public DecoratorStrategy(Func<T, object> func)
        {
            this.func = func;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            if(context.BuildKey.Type.Equals(typeof(T)))
            {
                context.Existing = func((T) context.Existing);
            }

        }
    }
}