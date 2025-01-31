using Azure;
using Azure.Storage;
using Azure.Storage.Files.Shares;
using Azure.Storage.Sas;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.FileShare;
using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using Cdcn.Enterprise.Library.Infrastructure.Extension;
using Cdcn.Enterprise.Library.Infrastructure.FileShare.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cdcn.Enterprise.Library.Infrastructure.FileShare
{
    public class FileShareService : IFileShareService
    {
        private readonly FileShareSetting _fileShareSetting;
        private readonly ILogger<FileShareService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public FileShareService(IOptions<FileShareSetting> fileShareSetting, ILogger<FileShareService> logger, IHttpClientFactory httpClientFactory)
        {
            _fileShareSetting = fileShareSetting.Value;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public Task<Result<bool>> DeleteFileAsync(string fileName, string directoryName = "", string shareName = IFileShareService.DefaultFileShareName)
        {
            throw new NotImplementedException();
        }

        public async Task<Result< Stream>> DownloadFileAsync(string fileName, string directoryName = "", string shareName = IFileShareService.DefaultFileShareName)
        {
            var file = await GetFileUrlAsync(fileName, directoryName, shareName);
            if (string.IsNullOrEmpty(file.Value))
                return Result.Failure<Stream>(FileShareErrors.GenericError);

            var client = _httpClientFactory.CreateClientWithPolicy();
            var stream = await client.GetAsync(file.Value);
            if (!stream.IsSuccessStatusCode)
                return Result.Failure<Stream>(FileShareErrors.GenericError);

            var data = await stream.Content.ReadAsStreamAsync();

            return Result.Success(data);
        }

        public Task<Result<HttpResponseMessage>> GetBlobAsync(string fileName, string folder = "", string shareName = IFileShareService.DefaultFileShareName)
        {
            throw new NotImplementedException();
        }

        private string GenerateSasToken(string shareName, string directoryName, string fileName, ShareFileClient client)
        {
            var filePath = string.IsNullOrEmpty(directoryName.Trim()) ? $"{fileName}" : $"{directoryName}/{fileName}";
            var sasBuilder = new ShareSasBuilder
            {
                ShareName = client.ShareName,
                FilePath = filePath,
                Resource = "f",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };

            sasBuilder.SetPermissions(ShareFileSasPermissions.Read);

            var storageSharedKeyCredential = new StorageSharedKeyCredential(
                _fileShareSetting.StorageAccountName, _fileShareSetting.StorageKey // storage key
            );

            return sasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();
        }

        public async Task<Result<string>> GetFileUrlAsync(string fileName, string directoryName = "", string shareName = IFileShareService.DefaultFileShareName)
        {
            ShareClient share = new ShareClient(_fileShareSetting.ConnectionString, shareName);
            await share.CreateIfNotExistsAsync();

            ShareFileClient file = null;
            if (!string.IsNullOrEmpty(directoryName))
            {
                ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
                await directory.CreateIfNotExistsAsync();
                file = directory.GetFileClient(fileName);
            }

            if (string.IsNullOrEmpty(directoryName))
            {
                file = share.GetRootDirectoryClient().GetFileClient(fileName);
            }

            if (await file.ExistsAsync())
            {
                var sasToken = GenerateSasToken(shareName, directoryName, fileName, file);
                var sasUri = $"{file.Uri}?{sasToken}";
                return Result.Success(sasUri);
            }
            else
            {
                return Result.Failure<string>(FileShareErrors.GenericError);
            }
        }

        public async Task<Result<string>> UploadFileAsync(string fileName, byte[] content, string directoryName = "", string shareName = IFileShareService.DefaultFileShareName)
        {
            ShareClient share = new ShareClient(_fileShareSetting.ConnectionString, shareName);
            await share.CreateIfNotExistsAsync();

            ShareFileClient file = null;
            if (!string.IsNullOrEmpty(directoryName))
            {
                ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
                await directory.CreateIfNotExistsAsync();
                file = directory.GetFileClient(fileName);
            }

            if (string.IsNullOrEmpty(directoryName))
            {
                file = share.GetRootDirectoryClient().GetFileClient(fileName);
            }

            using MemoryStream stream = new MemoryStream(content);
            await file.CreateAsync(stream.Length);
            await file.UploadRangeAsync(new HttpRange(0, stream.Length), stream);

            return await GetFileUrlAsync(fileName, directoryName, shareName);
        }
    }
}
