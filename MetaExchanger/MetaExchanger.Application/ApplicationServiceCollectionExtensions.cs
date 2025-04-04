using MetaExchanger.Application.Infrastructure;
using MetaExchanger.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace MetaExchanger.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICryptoExchangeService, CryptoExchangeService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
            return services;
        }

        public static IServiceCollection AddDatebase(this IServiceCollection services, string connString)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connString), ServiceLifetime.Scoped);
            services.AddScoped<DbInitializer>();
            return services;
        }
    }
}