using System;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    /// <summary>
    /// Расширения <see cref="IServiceCollection"/>
    /// </summary>
    public static class ConfigureServicesExtensions
    {
        /// <summary>
        /// Добавить сервис
        /// </summary>
        public static void AddService(this IServiceCollection services, Action<IServiceCollection> configure)
        {
            configure(services);
        }
    }
}