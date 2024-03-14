using AzureBlobStorage.Abstract;
using AzureBlobStorage.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AzureBlobStorage.Extensions
{
    /// <summary>
    /// The static class provides an ability to add Azure Blob Storage Service
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Azure Blob Service to DI container
        /// </summary>
        public static AzureBlobBuilder AddAzureBlobStorage(this IServiceCollection services)
        {
            services.TryAddScoped<IAzureBlobService, AzureBlobService>();

            return new AzureBlobBuilder(services);
        }
    }
}