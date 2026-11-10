using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RaceDayAPI.Services
{
    public class BlobService
    {
        private readonly string _connectionString;
        private readonly string _containerName = "event-banners";

        public BlobService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("AzureStorage")!;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return string.Empty;

            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // 1. Create if it doesn't exist
            await blobContainerClient.CreateIfNotExistsAsync();

            // 2. FORCE the container to be public (this fixes the old private container!)
            await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            var blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString().Replace("azurite", "localhost");
        }
    }
}
