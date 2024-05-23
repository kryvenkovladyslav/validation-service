using Domain.Abstract;
using AzureBlobStorage.Extensions;
using BusinessLayer.Implementation;
using BusinessLayer.Configuration.OptionsSetup;
using Microsoft.Extensions.DependencyInjection;
using BusinessLayer.Implementation.SettingsProviders;
using BusinessLayer.Implementation.ExternalResourceProviders;
using BusinessLayer.Implementation.ValidationStrategies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Xml;

namespace DependencyInjection.Extensions
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all required services for the application
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> collection for registering services</param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            ConfigureAllApplicationOptions(services);

            services.AddAzureBlobStorage().AddAzureAccessKeyAuthentication();

            TryAddScopedDocumentStorage<IDocumentStorage, AzureDocumentStorage>(services);
            TryAddScopedDocumentStorage<IAzureDocumentStorage, AzureDocumentStorage>(services);
            TryAddScopedDocumentValidators<IXmlDocumentValidator, XmlDocumentValidator, XmlReaderSettings>(services);
            TryAddScopedValidationSettingsCreator<IDtdValidationSettingsCreator, DtdValidationSettingsCreator>(services);
            TryAddScopedValidationSettingsCreator<IXmlSchemaValidationSettingsCreator, XmlSchemaValidationSettingsCreator>(services);

            TryAddScopedExternalXmlResourceProviders(services);
            TryAddScopedValidationXmlSettingProviders(services);
            TryAddScopedXmlValidationStrategies(services);

            TryAddScopedValidationManager<IXmlValidationManager, XmlValidationManager>(services);

            return services;
        }

        private static void TryAddScopedValidationManager<TInterface, TImplementation>(IServiceCollection services)
            where TImplementation : class, TInterface
            where TInterface : class, IXmlValidationManager
        {
            services.TryAddScoped<TInterface, TImplementation>();
        }

        private static void TryAddScopedValidationSettingsCreator<TInterface, TImplementation>(IServiceCollection services)
            where TImplementation : class, TInterface
            where TInterface : class, IXmlValidationSettingsCreator
        {
            services.TryAddScoped<TInterface, TImplementation>();
        }

        private static void TryAddScopedDocumentValidators<TInterface, TImplementation, TSettings>(IServiceCollection services)
            where TSettings : class
            where TImplementation : class, TInterface
            where TInterface : class, IDocumentValidator<TSettings>
        {
            services.TryAddScoped<TInterface, TImplementation>();
        }

        private static void TryAddScopedDocumentStorage<TInterface, TImplementation>(IServiceCollection services)
            where TImplementation : class, TInterface
            where TInterface : class, IDocumentStorage
        {
            services.TryAddScoped<TInterface, TImplementation>();
        }

        private static void TryAddScopedXmlValidationStrategies(IServiceCollection services)
        {
            services.AddScoped<IXmlDocumentValidationStrategy, DtdXmlDocumentValidationStrategy>();
            services.AddScoped<IXmlDocumentValidationStrategy, SchemaXmlDocumentValidationStrategy>();
        }

        private static void TryAddScopedValidationXmlSettingProviders(IServiceCollection services)
        {
            services.AddScoped<IValidationXmlSettingProvider<IXmlDocumentValidationStrategy>, AzureDtdValidationSettingsProvider>();
            services.AddScoped<IValidationXmlSettingProvider<IXmlDocumentValidationStrategy>, AzureXmlSchemaValidationSettingsProvider>();
        }

        private static void TryAddScopedExternalXmlResourceProviders(IServiceCollection services)
        {
            services.TryAddScoped<IAzureExternalDtdProvider, AzureExternalDtdProvider>();
            services.TryAddScoped<IAzureExternalXmlSchemaProvider, AzureExternalXmlSchemaProvider>();
        }

        private static void ConfigureAllApplicationOptions(IServiceCollection services)
        {
            services.ConfigureOptions<AzureIdentityOptionsSetup>();
            services.ConfigureOptions<AzureAccessKeyOptionsSetup>();
        }
    }
}