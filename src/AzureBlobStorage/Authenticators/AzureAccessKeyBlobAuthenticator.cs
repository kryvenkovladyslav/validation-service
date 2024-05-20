using Azure.Storage.Blobs;
using AzureBlobStorage.Options;
using AzureBlobStorage.Abstract;
using Microsoft.Extensions.Options;

namespace AzureBlobStorage.Authenticators
{
    /// <summary>
    /// The authenticator class for Authentication clients via access key
    /// </summary>
    public class AzureAccessKeyBlobAuthenticator : AzureBlobAuthenticator
    {
        /// <summary>
        /// The options for holding access key configuration
        /// </summary>
        protected AzureAccessKeyOptions AuthenticationOptions { get; private init; }

        /// <summary>
        /// The constructor for creating an instance using Azure Access Key options
        /// </summary>
        /// <param name="options">The Azure access key options</param>
        public AzureAccessKeyBlobAuthenticator(IOptionsMonitor<AzureAccessKeyOptions> options)
        {
            this.AuthenticationOptions = options.CurrentValue;
        }

        /// <summary>
        /// Authenticates the Azure Blob Client
        /// </summary>
        /// <returns>Authenticated client if the process of authentication does not fail</returns>
        public override BlobServiceClient AuthenticateClient()
        {
            return this.Authenticate(this.AuthenticationOptions.AccessKey);
        }
    }
}