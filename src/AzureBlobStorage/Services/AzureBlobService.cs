using System;
using System.Threading;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using AzureBlobStorage.Abstract;
using AzureBlobStorage.Exceptions;

namespace AzureBlobStorage.Services
{
    /// <summary>
    /// The Azure Blob Service for interacting with Azure Blob Storage
    /// </summary>
    public class AzureBlobService : IAzureBlobService
    {
        /// <summary>
        /// The options for holding Azure Blob Client
        /// </summary>
        private readonly BlobServiceClient client;

        /// <summary>
        /// The constructor for creating an instance using Azure authenticator options
        /// </summary>
        /// <param name="options">The Azure authenticator options</param>
        public AzureBlobService(IAzureBlobAuthenticator authenticator)
        {
            ArgumentNullException.ThrowIfNull(authenticator, nameof(authenticator));
            this.client = authenticator.AuthenticateClient();
        }

        /// <summary>
        /// Downloads required Azure Blob asynchronously
        /// </summary>
        /// <param name="requestModel">The model with required parameters</param>
        /// <returns>The task with downloaded Blob represented as binary data</returns>
        /// <exception cref="AzureBlobNotExistException">The exception is thrown if a specified blob does not exist</exception>
        public virtual async Task<BinaryData> DownloadBlobAsync(IBlobRequestModel requestModel, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(requestModel, nameof(requestModel));

            var containerClient = this.client.GetBlobContainerClient(requestModel.ContainerName);
            var blobClient = containerClient.GetBlobClient(requestModel.RequiredFilePath);

            try
            {
                var downloadedContent = await blobClient.DownloadContentAsync().ConfigureAwait(false);

                var binaryData = downloadedContent.Value.Content;
                return binaryData;
            }
            catch (Exception exception)
            {
                throw new AzureBlobNotExistException(exception);
            }
        }

        /// <summary>
        /// Checks if required Azure Blob exists
        /// </summary>
        /// <param name="requestModel">The model with required parameters</param>
        /// <returns>True if required Blob exists</returns>
        /// <exception cref="AzureBlobNotExistException">The exception is thrown if a specified blob does not exist</exception>
        public virtual async Task<bool> ExistsAsync(IBlobRequestModel requestModel, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(requestModel, nameof(requestModel));

            var containerClient = this.client.GetBlobContainerClient(requestModel.ContainerName);
            var blobClient = containerClient.GetBlobClient(requestModel.RequiredFilePath);

            try
            {
                return await blobClient.ExistsAsync(token).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw new AzureBlobNotExistException(exception);
            }
        }
    }
}