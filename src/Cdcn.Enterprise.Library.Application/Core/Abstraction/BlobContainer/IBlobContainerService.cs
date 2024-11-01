using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.BlobContainer
{
    public interface IBlobContainerService
    {
        Task<Result<string>> UploadBlobAsync(string blobName, byte[] content, string folder = "", string containerName = "swq");

        Task<Result<Stream>> DownloadBlobAsync(string blobName, string folder = "", string containerName = "swq");

        Task<Result<bool>> DeleteBlobAsync(string blobName, string folder = "", string containerName = "swq");

        Task<Result<string>> GetBlobUrlAsync(string blobName, string folder = "", string containerName = "swq");

        Task<Result<HttpResponseMessage>> GetBlobAsync(string blobName, string folder = "", string containerName = "swq");

    }
}
