using Microsoft.WindowsAzure.Storage.Blob;

namespace DataLayer
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString, string containerName);
    }
}
