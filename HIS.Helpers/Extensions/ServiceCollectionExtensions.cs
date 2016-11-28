using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using HIS.Data.Base.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace HIS.Helpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSingletonFactory<T, TFactory>(this IServiceCollection collection)
            where T : class where TFactory : class, IServiceFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Singleton);
        }

        /* transient and scoped variants omitted */

        private static IServiceCollection AddInternal<T, TFactory>(this IServiceCollection collection, Func<IServiceProvider, TFactory> factoryProvider, ServiceLifetime lifetime) where T : class where TFactory : class, IServiceFactory<T>
        {
            Func<IServiceProvider, object> factoryFunc = provider =>
            {
                var factory = factoryProvider(provider);
                return factory.Build();
            };
            var descriptor = new ServiceDescriptor(typeof(T), factoryFunc, lifetime);
            collection.Add(descriptor);
            return collection;
        }
        
    }
}
