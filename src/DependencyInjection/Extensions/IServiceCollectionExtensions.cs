using Domain.Abstract;
using AzureBlobStorage.Extensions;
using BusinessLayer.Implementation;
using BusinessLayer.Implementation.Validators;
using BusinessLayer.Configuration.OptionsSetup;
using Microsoft.Extensions.DependencyInjection;
using BusinessLayer.Implementation.SettingsProviders;

namespace DependencyInjection.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all required business services for the system
        /// </summary>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            ConfigureAllApplicationOptions(services);
            services.AddAzureBlobStorage().AddAzureAccessKeyAuthentication();

            services.AddScoped<IFileStorage, CloudFileStorage>();

            services.AddScoped<IValidationXmlSettingProvider<DtdDocumentValidationStrategy>, DtdValidationSettingsProvider>();
            services.AddScoped<IValidationXmlSettingProvider<SchemaDocumentValidationStrategy>, SchemaValidationSettingsProvider>();

            services.AddScoped<IDocumentValidationStrategy, DtdDocumentValidationStrategy>();
            services.AddScoped<IDocumentValidationStrategy, SchemaDocumentValidationStrategy>();

            return services;
        }

        /// <summary>
        /// Configures Azure Options
        /// </summary>
        private static IServiceCollection ConfigureAllApplicationOptions(IServiceCollection services)
        {
            services.ConfigureOptions<AzureIdentityOptionsSetup>();
            services.ConfigureOptions<AzureAccessKeyOptionsSetup>();

            return services;
        }
    }
}