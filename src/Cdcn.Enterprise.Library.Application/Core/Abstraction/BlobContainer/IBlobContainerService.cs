using Cdcn.Enterprise.Library.Domain.Primitives.Result;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.BlobContainer
{
    /// <summary>
    /// Interface for blob container service to handle blob operations.
    /// </summary>
    public interface IBlobContainerService
    {
        /// <summary>
        /// Uploads a blob to the specified container.
        /// </summary>
        /// <param name="blobName">The name of the blob.</param>
        /// <param name="content">The content of the blob as a byte array.</param>
        /// <param name="folder">The folder within the container (optional).</param>
        /// <param name="containerName">The name of the container (default is "Cdcn").</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the upload operation.</returns>
       

        const string DefaultContainerName = "Cdcn";
        Task<Result<string>> UploadBlobAsync(string blobName, byte[] content, string folder = "", string containerName = DefaultContainerName);

        /// <summary>
        /// Downloads a blob from the specified container.
        /// </summary>
        /// <param name="blobName">The name of the blob.</param>
        /// <param name="folder">The folder within the container (optional).</param>
        /// <param name="containerName">The name of the container (default is "Cdcn").</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the download operation.</returns>
        Task<Result<Stream>> DownloadBlobAsync(string blobName, string folder = "", string containerName = DefaultContainerName);

        /// <summary>
        /// Deletes a blob from the specified container.
        /// </summary>
        /// <param name="blobName">The name of the blob.</param>
        /// <param name="folder">The folder within the container (optional).</param>
        /// <param name="containerName">The name of the container (default is "Cdcn").</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the delete operation.</returns>
        Task<Result<bool>> DeleteBlobAsync(string blobName, string folder = "", string containerName = DefaultContainerName);

        /// <summary>
        /// Gets the URL of a blob in the specified container.
        /// </summary>
        /// <param name="blobName">The name of the blob.</param>
        /// <param name="folder">The folder within the container (optional).</param>
        /// <param name="containerName">The name of the container (default is "Cdcn").</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the get URL operation.</returns>
        Task<Result<string>> GetBlobUrlAsync(string blobName, string folder = "", string containerName = DefaultContainerName);

        /// <summary>
        /// Gets the blob as an HTTP response message from the specified container.
        /// </summary>
        /// <param name="blobName">The name of the blob.</param>
        /// <param name="folder">The folder within the container (optional).</param>
        /// <param name="containerName">The name of the container (default is "Cdcn").</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result of the get blob operation.</returns>
        Task<Result<HttpResponseMessage>> GetBlobAsync(string blobName, string folder = "", string containerName = DefaultContainerName);
    }
}
