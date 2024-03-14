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
    /// A Cloud implementation of a file storage on Azure Blob
    /// </summary>
    public class CloudFileStorage : IFileStorage
    {
        /// <summary>
        /// The service for interacting with Azure Blob Storage
        /// </summary>
        private readonly IAzureBlobService azureBlobService;

        /// <summary>
        /// The options for configuring file storage
        /// </summary>
        private readonly FileStoreConfigurationOptions fileStorageConfiguration;

        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <param name="azureBlobService"></param>
        /// <param name="fileStorageConfiguration"></param>
        /// <exception cref="ArgumentNullException">The ArgumentNullException is thrown if Azure service is null or options in not provided</exception>
        public CloudFileStorage(IAzureBlobService azureBlobService, IOptionsMonitor<FileStoreConfigurationOptions> fileStorageConfiguration)
        {
            ArgumentNullException.ThrowIfNull(fileStorageConfiguration, nameof(fileStorageConfiguration));

            this.azureBlobService = azureBlobService ?? throw new ArgumentNullException(nameof(azureBlobService));

            this.fileStorageConfiguration = fileStorageConfiguration.CurrentValue;
        }

        /// <summary>
        /// Asynchronously finds a file in a file store
        /// </summary>
        /// <param name="fullPath">The full path to a required file</param>
        /// <returns>The document stream if the file exists in a store</returns>
        public virtual async Task<Stream> FindByFullPathAsync(string fullPath)
        {
            var binaryData = await this.azureBlobService.DownloadBlobAsync(new BlobRequestModel
            {
                ContainerName = this.fileStorageConfiguration.ContainerName,
                RequiredFilePath = fullPath
            });

            return binaryData.ToStream();
        }

        /// <summary>
        /// Asynchronously checks if a file exists in a file store
        /// </summary>
        /// <param name="fullPath">The full path to a required file</param>
        /// <returns>True if the specified file exists</returns>
        public virtual Task<bool> FindExistsAsync(string fullPath)
        {
            return this.azureBlobService.ExistsAsync(new BlobRequestModel
            {
                ContainerName = this.fileStorageConfiguration.ContainerName,
                RequiredFilePath = fullPath
            });
        }
    }
}