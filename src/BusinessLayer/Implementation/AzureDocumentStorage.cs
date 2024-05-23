using System;
using System.IO;
using Domain.Abstract;
using BusinessLayer.Models;
using System.Threading.Tasks;
using AzureBlobStorage.Abstract;
using Microsoft.Extensions.Options;
using BusinessLayer.Configuration.Options;

namespace BusinessLayer.Implementation
{
    /// <summary>
    /// Provides method for extracting documents from Azure Blob Storage
    /// </summary>
    public class AzureDocumentStorage : IAzureDocumentStorage
    {
        /// <summary>
        /// The service for extracting resources from Azure Blob Storage
        /// </summary>
        public readonly IAzureBlobService azureBlobService;

        /// <summary>
        /// Provides a configuration for a storage of documents
        /// </summary>
        private readonly DocumentStorageConfigurationOptions documentStorageOptions;

        /// <summary>
        /// Initializes the class using IAzureBlobService and IOptionsMonitor
        /// </summary>
        /// <param name="azureBlobService">The service for extracting resources from Azure Blob Storage</param>
        /// <param name="documentStorageOptions">The configuration for configuring Azure Blob Storage dependencies</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if one of the arguments is not provided</exception>
        public AzureDocumentStorage(IAzureBlobService azureBlobService, IOptionsMonitor<DocumentStorageConfigurationOptions> documentStorageOptions)
        {
            ArgumentNullException.ThrowIfNull(documentStorageOptions, nameof(documentStorageOptions));
            this.azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));

            this.documentStorageOptions = documentStorageOptions.CurrentValue;
        }

        /// <summary>
        /// Asynchronously extracts required document from Azure Blob Storage
        /// </summary>
        /// <param name="fullPath">A path to a required document</param>
        /// <returns>Created stream representing extracted document</returns>
        public virtual async Task<Stream> FindByFullPathAsync(string fullPath)
        {
            var binaryData = await this.azureBlobService.DownloadBlobAsync(new BlobRequestModel
            {
                ContainerName = this.documentStorageOptions.ContainerName,
                RequiredFilePath = fullPath
            });

            return binaryData.ToStream();
        }

        /// <summary>
        /// Asynchronously checks if the specified document exists
        /// </summary>
        /// <param name="fullPath">A path to a required document</param>
        /// <returns>True if the specified document exists, otherwise False</returns>
        public virtual Task<bool> ExistsAsync(string fullPath)
        {
            return this.azureBlobService.ExistsAsync(new BlobRequestModel
            {
                ContainerName = this.documentStorageOptions.ContainerName,
                RequiredFilePath = fullPath
            });
        }
    }
}