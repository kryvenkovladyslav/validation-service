using Azure.Storage.Blobs;
using AzureBlobStorage.Options;
using AzureBlobStorage.Abstract;
using Microsoft.Extensions.Options;

namespace AzureBlobStorage.Authenticators
{
    /// <summary>
    /// The authenticator class for Authentication clients via access key
    /// </summary>
    public class AzureAccessKeyBlobAuthenticator : AzureBlobAuthenticator, IAzureBlobAuthenticator
    {
        /// <summary>
        /// The options for holding access key configuration
        /// </summary>
        private readonly AzureAccessKeyOptions options;

        /// <summary>
        /// The constructor for creating an instance using Azure Access Key options
        /// </summary>
        /// <param name="options">The Azure access key options</param>
        public AzureAccessKeyBlobAuthenticator(IOptionsMonitor<AzureAccessKeyOptions> options)
        {
            this.options = options.CurrentValue;
        }

        /// <summary>
        /// Au
        /// </summary>
        /// <returns></returns>
        public virtual BlobServiceClient AuthenticateClient()
        {
            return base.Authenticate(this.options.AccessKey);
        }
    }
}