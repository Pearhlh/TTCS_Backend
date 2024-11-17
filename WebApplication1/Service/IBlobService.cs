namespace WebApplication1.Service
{
    public interface IBlobService
    {
        Task<Uri> UploadFileBlobAsync(string blobContainerName, Stream content, string contentType, string fileName);
        Task<Boolean> DeleteFileBlobAsync(string blobContainerName, string fileName);
        Task<Uri> UploadStringBlobAsync(string blobContainerName, string content, string contentType, string fileName);
        Task<string> GetBlobContentAsync(string blobContainerName, string blobName);
        Task<Boolean> RenameFileBlobAsync(string blobContainerName, string fileNameOld, string fileNameNew);
    }
}
