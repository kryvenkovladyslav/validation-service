using AzureBlobStorage.Abstract;
using Microsoft.Extensions.Options;
using BusinessLayer.Configuration.Options;

namespace BusinessLayer.Implementation.ExternalResourceProviders
{
    /// <summary>
    /// Provides methods for extracting external DTD resources from Azure Blob Storage
    /// </summary>
    public class AzureExternalDtdProvider : AzureExternalResourceProvider, IAzureExternalDtdProvider
    {
        /// <summary>
        /// Initializes the class using IAzureBlobService and IOptionsMonitor
        /// </summary>
        /// <param name="azureBlobService">The service for extracting DTD resources from Azure Blob Storage</param>
        /// <param name="azureExternalProviderOprtions">The configuration for configuring Azure Blob Storage dependencies</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if azureBlobService or fileStorageConfiguration is not provided</exception>
        public AzureExternalDtdProvider(IAzureBlobService azureBlobService, 
            IOptionsMonitor<AzureExternalResourceProviderConfigurationOptions> azureExternalProviderOprtions) : base(azureBlobService, azureExternalProviderOprtions) { }
    }
}