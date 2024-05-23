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
    public class AzureCredentialsBlobAuthenticator : AzureBlobAuthenticator
    {
        /// <summary>
        /// The options for holding Azure credentials configuration
        /// </summary>
        protected AzureIdentityOptions AuthenticationOptions { get; private init; }

        /// <summary>
        /// The constructor for creating an instance using Azure credentials options
        /// </summary>
        /// <param name="options">The Azure credentials options</param>
        public AzureCredentialsBlobAuthenticator(IOptionsMonitor<AzureIdentityOptions> options)
        {
            this.AuthenticationOptions = options.CurrentValue;
        }

        /// <summary>
        /// Authenticate the Azure Blob Client
        /// </summary>
        /// <param name="arguments">The parameters could be thrown in constructor</param>
        /// <returns>Authenticated client if the process of authentication does not fail</returns>
        public override BlobServiceClient AuthenticateClient()
        {
            var parameters = new object[]
            {
                new Uri(this.AuthenticationOptions.FullyQualifiedNamespace),
                new DefaultAzureCredential(),
                new BlobClientOptions()
            };

            return this.Authenticate(parameters);
        }
    }
}