using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using Example.Repository.Interface;
using Example.Repository;
using Example.Domain;

namespace Example.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return _container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ExampleContext>(new PerRequestLifetimeManager());
            container.RegisterType<Example.Infrastructure.UnityContainer>();
            //container.RegisterType<,
            var types = typeof(UserRepository).Assembly.GetTypes().Where(t => t.IsInterface == false && t.IsGenericType == false);
            foreach (var item in types)
            {
                Debug.WriteLine(item.GetInterfaces().FirstOrDefault().FullName);
            }
            container.RegisterTypes(types, m => m.GetInterfaces().Where(t => t.Name.Contains(m.Name)));
            foreach (var item in container.Registrations)
            {
                Debug.WriteLine("Mapped:{0}-->ToType:{1}", item.MappedToType.FullName, item.RegisteredType.FullName);
            }
        }
    }
}