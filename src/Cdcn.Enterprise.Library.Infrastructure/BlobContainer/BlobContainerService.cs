using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.BlobContainer;
using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using Cdcn.Enterprise.Library.Infrastructure.BlobContainer.Settings;
using Cdcn.Enterprise.Library.Infrastructure.Extension;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cdcn.Enterprise.Library.Infrastructure.BlobContainer
{
    public class BlobContainerService : IBlobContainerService
    {
        private readonly BlobContainerSetting _blobContainerSetting;
        private readonly ILogger<BlobContainerService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BlobContainerService(IOptions<BlobContainerSetting> blobContainerSetting, ILogger<BlobContainerService> logger, IHttpClientFactory httpClientFactory)
        {
            _blobContainerSetting = blobContainerSetting.Value;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        private async Task<BlobContainerClient> GetBlobContainerClientAsync(string containerName)
        {
            var blobServiceClient = new BlobServiceClient(_blobContainerSetting.ConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            return blobContainerClient;
        }

        private string GenerateSasToken(BlobClient blobClient)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var storageSharedKeyCredential = new StorageSharedKeyCredential(
                _blobContainerSetting.StorageAccountName, _blobContainerSetting.StorageKey // storage key
            );

            return sasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();
        }

        private string GetFileName(string fileName, string folder = "")
        {
            return string.IsNullOrEmpty(folder) ? fileName.Trim() : folder.Trim() + "\\" + fileName.Trim();
        }

        public async Task<Result<bool>> DeleteBlobAsync(string blobName, string folder = "", string containerName = BlobContainerSetting.DefaultContainerName)
        {
            try
            {

                var blobFile = GetFileName(blobName, folder);
                var blobContainerClient = await GetBlobContainerClientAsync(containerName);
                var blobClient = blobContainerClient.GetBlobClient(blobFile);

                var isDeleted = await blobClient.DeleteIfExistsAsync();
                if (!isDeleted)
                    return Result.Failure<bool>(BlobContainerErrors.ErrorWhenDeleting);

                return Result.Success<bool>(isDeleted);


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting blob: {ex.Message}");
                return Result.Failure<bool>(BlobContainerErrors.ErrorWhenDeleting);
            }
        }

        public async Task<Result<string>> GetBlobUrlAsync(string blobName, string folder = "", string containerName = BlobContainerSetting.DefaultContainerName)
        {
            var blobFile = GetFileName(blobName, folder);
            var containerClient = await GetBlobContainerClientAsync(containerName);
            var blobClient = containerClient.GetBlobClient(blobFile);

            if (await blobClient.ExistsAsync())
            {
                var sasToken = GenerateSasToken(blobClient);
                var sasUri = $"{blobClient.Uri}?{sasToken}";
               
                return Result.Success<string>(sasUri);
            }
            else
            {
                return Result.Failure<string>(BlobContainerErrors.FileNotExist);
            }
        }

        public async Task<Result<Stream>> DownloadBlobAsync(string blobName, string folder = "", string containerName = BlobContainerSetting.DefaultContainerName)
        {
            try
            {
                var blobFile = GetFileName(blobName, folder);
                var blobContainerClient = await GetBlobContainerClientAsync(containerName);
                var blobClient = blobContainerClient.GetBlobClient(blobFile);

                var downloadInfo = await blobClient.DownloadAsync();              
                return Result.Success<Stream>(downloadInfo.Value.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error downloading blob: {ex.Message}");
                return Result.Failure<Stream>(BlobContainerErrors.FileNotExist);
            }
        }

        public async Task< Result<string>> UploadBlobAsync(string blobName, byte[] content, string folder = "", string containerName = BlobContainerSetting.DefaultContainerName)
        {
            try
            {
                var blobFile = GetFileName(blobName, folder);
                var blobContainerClient = await GetBlobContainerClientAsync(containerName);
                var blobClient = blobContainerClient.GetBlobClient(blobFile);

                using (var memoryStream = new MemoryStream(content))
                {
                    await blobClient.UploadAsync(memoryStream, true);
                }

                return await GetBlobUrlAsync(blobName, folder, containerName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading blob: {ex.Message}");
                return Result.Failure<string>(BlobContainerErrors.FileNotExist);
            }
        }

        public async Task<Result<HttpResponseMessage>> GetBlobAsync(string blobName, string folder = "", string containerName = BlobContainerSetting.DefaultContainerName)
        {
            var blobFile = GetFileName(blobName, folder);
            var client = _httpClientFactory.CreateClientWithPolicy();
            var containerClient = await GetBlobContainerClientAsync(containerName);
            var blobClient = containerClient.GetBlobClient(blobFile);
            var sasUri = "";

            if (!await blobClient.ExistsAsync())
            {
                return Result.Failure<HttpResponseMessage>(BlobContainerErrors.FileNotExist); 
            }

            var sasToken = GenerateSasToken(blobClient);
            sasUri = $"{blobClient.Uri}?{sasToken}";
            if (string.IsNullOrEmpty(sasUri))
            {
                return Result.Failure<HttpResponseMessage>(BlobContainerErrors.FileNotExist); 
            }

            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(sasUri);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                return Result.Failure<HttpResponseMessage>(BlobContainerErrors.FileNotExist); ;            }

            return Result.Success<HttpResponseMessage>(response);
        }
    }
}
