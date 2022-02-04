using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using GISA.BPM.Infrastructure.Storage.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GISA.BPM.Infrastructure.Storage.Repositories
{
    public class S3StorageRepository : IStorageRepository
    {
        private readonly AmazonS3Client _amazonS3Client;
        private readonly TransferUtility _fileTransferUtility;
        private readonly IStorageConfiguration _storageConfiguration;

        public S3StorageRepository(IStorageConfiguration storageConfiguration)
        {
            _amazonS3Client = new AmazonS3Client(storageConfiguration.GetAccessKey(),
                storageConfiguration.GetSecretKey(), RegionEndpoint.GetBySystemName(storageConfiguration.GetRegion()));
            _fileTransferUtility = new TransferUtility(_amazonS3Client);
            _storageConfiguration = storageConfiguration;
        }

        public Task<string> GetFileUrlAsync(Guid fileIdentifier)
        {
            return Task.Run(() => $"{_storageConfiguration.GetBucketUrlBase()}{fileIdentifier}");
        }

        public Task RemoveFileAsync(Guid fileIdentifier)
        {
            return _amazonS3Client.DeleteObjectAsync(_storageConfiguration.GetBucketName(), $"{fileIdentifier}");
        }

        public Task<Tuple<Guid, string>> ReplaceFileAsync(Guid fileIdentifier, IFormFile file)
        {
            return StartUploadFileAsync(file, fileIdentifier);
        }

        public Task<Tuple<Guid, string>> UploadFileAsync(IFormFile file)
        {
            return StartUploadFileAsync(file);
        }

        private async Task<Tuple<Guid, string>> StartUploadFileAsync(IFormFile file, Guid? fileIdentifier = null)
        {
            TransferUtilityUploadRequest transferUtilityUploadRequest;
            if (!fileIdentifier.HasValue) fileIdentifier = Guid.NewGuid();

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);

                transferUtilityUploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = memoryStream,
                    Key = $"{fileIdentifier}",
                    BucketName = _storageConfiguration.GetBucketName(),
                    CannedACL = S3CannedACL.PublicRead
                };

                await _fileTransferUtility.UploadAsync(transferUtilityUploadRequest);
            }

            return new Tuple<Guid, string>(fileIdentifier.Value, $"{_storageConfiguration.GetBucketUrlBase()}{fileIdentifier}");
        }
    }
}
