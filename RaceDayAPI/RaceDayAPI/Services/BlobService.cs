using Azure.Storage.Blobs;

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

            // Create the container if it doesn't exist
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);

            return blobClient.Uri.ToString();
        }
    }
}
