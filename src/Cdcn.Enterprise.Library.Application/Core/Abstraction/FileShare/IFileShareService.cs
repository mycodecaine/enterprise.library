using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.FileShare
{
    public interface IFileShareService
    {
        Task<Result<string>> UploadFileAsync(string fileName, byte[] content, string directory = "", string shareName = "swq");

        Task<Result<Stream>> DownloadFileAsync(string fileName, string directoryName = "", string shareName = "swq");

        Task<Result<bool>> DeleteFileAsync(string fileName, string directory = "", string shareName = "swq");

        Task<Result<string>> GetFileUrlAsync(string fileName, string directory = "", string shareName = "swq");
    }
}
