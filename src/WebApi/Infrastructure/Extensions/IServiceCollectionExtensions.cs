using Microsoft.Extensions.Configuration;
using BusinessLayer.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAllApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileStoreConfigurationOptions>(configuration.GetSection(FileStoreConfigurationOptions.Position));
            services.Configure<AzureIdentityConfigurationOptions>(configuration.GetSection(AzureIdentityConfigurationOptions.Position));
            services.Configure<AzureAccessKeyConfigurationOptions>(configuration.GetSection(AzureAccessKeyConfigurationOptions.Position));

            return services;
        }
    }
}