using AzureBlobStorage.Abstract;
using Microsoft.Extensions.Options;
using BusinessLayer.Configuration.Options;

namespace BusinessLayer.Implementation.ExternalResourceProviders
{
    /// <summary>
    /// Provides methods for extracting external XML Schema resources from Azure Blob Storage
    /// </summary>
    public class AzureExternalXmlSchemaProvider : AzureExternalResourceProvider, IAzureExternalXmlSchemaProvider
    {
        /// <summary>
        /// Initializes the class using IAzureBlobService and IOptionsMonitor
        /// </summary>
        /// <param name="azureBlobService">The service for extracting XMLSchema resources from Azure Blob Storage</param>
        /// <param name="azureExternalResourceProviderOptions">The configuration for configuring Azure Blob Storage dependencies</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if azureBlobService or fileStorageConfiguration is not provided</exception>
        public AzureExternalXmlSchemaProvider(IAzureBlobService azureBlobService, 
            IOptionsMonitor<AzureExternalResourceProviderConfigurationOptions> azureExternalResourceProviderOptions) : base(azureBlobService, azureExternalResourceProviderOptions) { }
    }
}