using System;
using Azure.Storage.Blobs;
using AzureBlobStorage.Abstract;

namespace AzureBlobStorage.Authenticators
{
    /// <summary>
    /// The basic abstract class for Authentication clients
    /// </summary>
    public abstract class AzureBlobAuthenticator : IAzureBlobAuthenticator
    {
        /// <summary>
        /// The Azure Blob Client
        /// </summary>
        protected BlobServiceClient Client { get; private set; }

        /// <summary>
        /// Authenticates the Azure Blob Client
        /// </summary>
        /// <returns>Authenticated client if the process of authentication does not fail</returns>
        public abstract BlobServiceClient AuthenticateClient();

        /// <summary>
        /// Authenticate the Azure Blob Client
        /// </summary>
        /// <param name="arguments">The parameters could be thrown in constructor</param>
        /// <returns>Authenticated client if the process of authentication does not fail</returns>
        protected virtual BlobServiceClient Authenticate(params object[] arguments)
        {
            if (this.Client == null)
            {
                this.Client = (BlobServiceClient)Activator.CreateInstance(typeof(BlobServiceClient), arguments);
            }

            return this.Client;
        }
    }
}