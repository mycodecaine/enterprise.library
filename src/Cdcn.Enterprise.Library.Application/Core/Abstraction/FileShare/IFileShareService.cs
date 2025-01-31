using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.FileShare
{
    /// <summary>
    /// Provides an abstraction for file sharing services.
    /// </summary>
    public interface IFileShareService
    {
        const string DefaultFileShareName = "Cdcn";

        /// <summary>
        /// Uploads a file asynchronously to the specified directory and share.
        /// </summary>
        /// <param name="fileName">The name of the file to upload.</param>
        /// <param name="content">The content of the file as a byte array.</param>
        /// <param name="directory">The directory to upload the file to. Default is an empty string.</param>
        /// <param name="shareName">The name of the file share. Default is "Cdcn".</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/> with the URL of the uploaded file.</returns>
        Task<Result<string>> UploadFileAsync(string fileName, byte[] content, string directory = "", string shareName = DefaultFileShareName);

        /// <summary>
        /// Downloads a file asynchronously from the specified directory and share.
        /// </summary>
        /// <param name="fileName">The name of the file to download.</param>
        /// <param name="directoryName">The directory to download the file from. Default is an empty string.</param>
        /// <param name="shareName">The name of the file share. Default is "Cdcn".</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/> with the file stream.</returns>
        Task<Result<Stream>> DownloadFileAsync(string fileName, string directoryName = "", string shareName = DefaultFileShareName);

        /// <summary>
        /// Deletes a file asynchronously from the specified directory and share.
        /// </summary>
        /// <param name="fileName">The name of the file to delete.</param>
        /// <param name="directory">The directory to delete the file from. Default is an empty string.</param>
        /// <param name="shareName">The name of the file share. Default is "Cdcn".</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/> indicating whether the deletion was successful.</returns>
        Task<Result<bool>> DeleteFileAsync(string fileName, string directory = "", string shareName = DefaultFileShareName);

        /// <summary>
        /// Gets the URL of a file asynchronously from the specified directory and share.
        /// </summary>
        /// <param name="fileName">The name of the file to get the URL for.</param>
        /// <param name="directory">The directory to get the file URL from. Default is an empty string.</param>
        /// <param name="shareName">The name of the file share. Default is "Cdcn".</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/> with the file URL.</returns>
        Task<Result<string>> GetFileUrlAsync(string fileName, string directory = "", string shareName = DefaultFileShareName);
    }
}
