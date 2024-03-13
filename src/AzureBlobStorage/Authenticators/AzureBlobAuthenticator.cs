using System;
using Azure.Storage.Blobs;

namespace AzureBlobStorage.Authenticators
{
    /// <summary>
    /// The basic abstract class for Authentication clients
    /// </summary>
    public abstract class AzureBlobAuthenticator
    {
        /// <summary>
        /// The Azure Blob Client
        /// </summary>
        private BlobServiceClient client;

        /// <summary>
        /// Authenticate the Azure Blob Client
        /// </summary>
        /// <param name="arguments">The parameters could be thrown in constructor</param>
        /// <returns>Authenticated client if the process of authentication does not fail</returns>
        protected virtual BlobServiceClient Authenticate(params object[] arguments)
        {
            if (this.client == null)
            {
                this.client = (BlobServiceClient)Activator.CreateInstance(typeof(BlobServiceClient), arguments);
            }

            return this.client;
        }
    }
}