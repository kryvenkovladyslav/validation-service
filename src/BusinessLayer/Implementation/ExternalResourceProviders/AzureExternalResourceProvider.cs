using System;
using System.IO;
using BusinessLayer.Models;
using AzureBlobStorage.Abstract;
using BusinessLayer.Configuration.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation.ExternalResourceProviders
{
    /// <summary>
    /// Provides methods for extracting some external resources from Azure Blob Storage
    /// </summary>
    public class AzureExternalResourceProvider : IAzureExternalResourceProvider
    {
        /// <summary>
        /// The service for extracting resources from Azure Blob Storage
        /// </summary>
        protected IAzureBlobService AzureBlobService { get; private init; }

        /// <summary>
        /// The configuration for configuring Azure Blob Storage dependencies 
        /// </summary>
        protected AzureExternalResourceProviderConfigurationOptions AzureExternalProviderOptions { get; private init; }

        /// <summary>
        /// Initializes the class using IAzureBlobService and IOptionsMonitor
        /// </summary>
        /// <param name="azureBlobService">The service for extracting resources from Azure Blob Storage</param>
        /// <param name="providerOptions">The configuration for configuring Azure Blob Storage dependencies</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if azureBlobService or fileStorageConfiguration is not provided</exception>
        public AzureExternalResourceProvider(IAzureBlobService azureBlobService, IOptionsMonitor<AzureExternalResourceProviderConfigurationOptions> azureExternalProviderOprtions)
        {
            ArgumentNullException.ThrowIfNull(azureExternalProviderOprtions, nameof(azureExternalProviderOprtions));
            this.AzureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));

            this.AzureExternalProviderOptions = azureExternalProviderOprtions.CurrentValue;
        }

        /// <summary>
        /// Asynchronously extracts required DTD resource
        /// </summary>
        /// <param name="resourceFullPath">A path to a required DTD resource</param>
        /// <returns>Created stream representing extracted DTD resource</returns>
        public virtual async Task<Stream> ProvideResourceAsync(string resourceFullPath)
        {
            var binaryData = await this.AzureBlobService.DownloadBlobAsync(new BlobRequestModel
            {
                ContainerName = this.AzureExternalProviderOptions.ContainerName,
                RequiredFilePath = resourceFullPath
            });

            return binaryData.ToStream();
        }

        /// <summary>
        /// Asynchronously checks if the specified DTD resource exists
        /// </summary>
        /// <param resourceFullPath="resourceFullPath">A path to a required DTD resource</param>
        /// <returns>True if the specified resource exists, otherwise False</returns>
        public virtual Task<bool> ResourceExistsAsync(string resourceFullPath)
        {
            return this.AzureBlobService.ExistsAsync(new BlobRequestModel
            {
                ContainerName = this.AzureExternalProviderOptions.ContainerName,
                RequiredFilePath = resourceFullPath
            });
        }
    }
}