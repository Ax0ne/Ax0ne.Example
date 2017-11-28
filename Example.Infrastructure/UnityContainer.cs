/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 19:21:44
 *  FileName:UnityContainer.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Example.Infrastructure
{
    public class UnityContainer
    {
        private static IUnityContainer _container;
        static UnityContainer()
        {
            _container = null; // new Microsoft.Practices.Unity
        }
        //public UnityContainer(IUnityContainer container)
        //{
        //    _container = container;
        //}
        [Dependency]
        public static IUnityContainer Container { get { return _container; } }
        public static T Resolve<T>(params ResolverOverride[] overrides)
        {
            return _container.Resolve<T>(overrides);
        }
    }
    //public static class MyUnityContainerExtensions
    //{
    //    public static T Resolve<T>(this UnityContainer container, params ResolverOverride[] overrides)
    //    {
    //        return container.Container.Resolve<T>(overrides);
    //    }
    //}
}
