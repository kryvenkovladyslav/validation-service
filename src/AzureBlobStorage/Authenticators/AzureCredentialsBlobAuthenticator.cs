using System;
using Azure.Identity;
using Azure.Storage.Blobs;
using AzureBlobStorage.Options;
using AzureBlobStorage.Abstract;
using Microsoft.Extensions.Options;

namespace AzureBlobStorage.Authenticators
{
    /// <summary>
    /// The authenticator class for Authentication clients via Azure Credentials
    /// </summary>
    public class AzureCredentialsBlobAuthenticator : AzureBlobAuthenticator, IAzureBlobAuthenticator
    {
        /// <summary>
        /// The options for holding Azure credentials configuration
        /// </summary>
        private readonly AzureIdentityOptions options;

        /// <summary>
        /// The constructor for creating an instance using Azure credentials options
        /// </summary>
        /// <param name="options">The Azure credentials options</param>
        public AzureCredentialsBlobAuthenticator(IOptionsMonitor<AzureIdentityOptions> options)
        {
            this.options = options.CurrentValue;
        }

        /// <summary>
        /// Authenticate the Azure Blob Client
        /// </summary>
        /// <param name="arguments">The parameters could be thrown in constructor</param>
        /// <returns>Authenticated client if the process of authentication does not fail</returns>
        public virtual BlobServiceClient AuthenticateClient()
        {
            var parameters = new object[]
            {
                new Uri(this.options.FullyQualifiedNamespace),
                new DefaultAzureCredential(),
                new BlobClientOptions()
            };

            return base.Authenticate(parameters);
        }
    }
}