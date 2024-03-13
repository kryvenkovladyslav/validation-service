using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureBlobStorage.Abstract
{
    /// <summary>
    /// The interface for dealing with Azure Blob Service
    /// </summary>
    public interface IAzureBlobService
    {
        /// <summary>
        /// Downloads required Azure Blob asynchronously
        /// </summary>
        /// <param name="requestModel">The model with required parameters</param>
        /// <returns>The task with downloaded Blob represented as binary data</returns>
        public Task<BinaryData> DownloadBlobAsync(IBlobRequestModel requestModel, CancellationToken token = default);

        /// <summary>
        /// Checks if required Azure Blob exists
        /// </summary>
        /// <param name="requestModel">The model with required parameters</param>
        /// <returns>True if required Blob exists</returns>
        public Task<bool> ExistsAsync(IBlobRequestModel requestModel, CancellationToken token = default);
    }
}