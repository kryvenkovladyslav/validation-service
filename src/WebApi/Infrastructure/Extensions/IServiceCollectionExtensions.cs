using Microsoft.Extensions.Configuration;
using BusinessLayer.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureAllApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureIdentityConfigurationOptions>(configuration.GetSection(AzureIdentityConfigurationOptions.Position));
            services.Configure<AzureAccessKeyConfigurationOptions>(configuration.GetSection(AzureAccessKeyConfigurationOptions.Position));
            services.Configure<DocumentStorageConfigurationOptions>(configuration.GetSection(DocumentStorageConfigurationOptions.Position));
            services.Configure<AzureExternalResourceProviderConfigurationOptions>(configuration.GetSection(AzureExternalResourceProviderConfigurationOptions.Position));

            return services;
        }
    }
}