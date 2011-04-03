using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class PostBuildUpExpression<T> : Expression where T : class
    {
        private Action<IUnityContainer, T> action;
        private Func<IUnityContainer, T, object> decoratorFunc;

        /// <summary>
        /// Call a method or a property setter on the instance after it is constructed.
        /// </summary>
        /// <param name="action"></param>
        public void Call(Action<IUnityContainer, T> action)
        {
            this.action = action;
        }

        /// <summary>
        /// Decorate the instance with another class after it is constructed.
        /// </summary>
        /// <param name="func">The function used to construct the class to decorate with.</param>
        public void DecorateWith(Func<IUnityContainer, T, object> func)
        {
            decoratorFunc = func;
        }

        internal override void Execute(IUnityContainer container)
        {
            if (decoratorFunc != null)
                container.AddExtension(new DecoratorExtension<T>(decoratorFunc));

            if (action != null)
                container.AddExtension(new PostBuildUpActionExtension<T>(action));
        }
    }
}