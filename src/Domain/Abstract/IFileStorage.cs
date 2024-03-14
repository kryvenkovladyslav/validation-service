using System.IO;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    /// <summary>
    /// The storage the can asynchronously perform operations to a store
    /// </summary>
    public interface IFileStorage
    {
        /// <summary>
        /// Asynchronously checks if a file exists in a file store
        /// </summary>
        /// <param name="fullPath">The full path to a required file</param>
        /// <returns>True if the specified file exists</returns>
        public Task<bool> FindExistsAsync(string fullPath);

        /// <summary>
        /// Asynchronously finds a file in a file store
        /// </summary>
        /// <param name="fullPath">The full path to a required file</param>
        /// <returns>The document stream if the file exists in a store</returns>
        public Task<Stream> FindByFullPathAsync(string fullPath);
    }
}