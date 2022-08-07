using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using ByLearning.SSO.Application.ViewModels;
using ByLearning.SSO.Domain.ViewModels.Settings;
using System;
using System.IO;
using System.Threading.Tasks;
using ByLearning.Domain.Core.Util;

namespace ByLearning.SSO.Application.CloudServices.Storage
{
    public class AwsStorageService : IStorageService
    {
        private readonly StorageSettings _privateSettings;

        public AwsStorageService(StorageSettings privateSettings)
        {
            _privateSettings = privateSettings;
        }

        public async Task<string> Upload(FileUploadViewModel image)
        {
            var client = GetClient();

            var putRequest = new PutObjectRequest
            {
                BucketName = _privateSettings.StorageName,

                Key = image.Filename,
                ContentType = image.FileType,
                InputStream = new MemoryStream(Convert.FromBase64String(image.Value)),
                CannedACL = S3CannedACL.PublicRead,
            };
            if (!image.VirtualLocation.IsNullOrEmpty())
                putRequest.Metadata.Add("Location", image.VirtualLocation);
            var response = await client.PutObjectAsync(putRequest);

            return $"https://{_privateSettings.StorageName}.s3.{_privateSettings.Region}.amazonaws.com/{image.Filename}";
        }

        private IAmazonS3 GetClient()
        {
            return new AmazonS3Client(new BasicAWSCredentials(_privateSettings.Username, _privateSettings.Password), RegionEndpoint.GetBySystemName(_privateSettings.Region));

        }


        public async Task RemoveFile(string fileName, string virtualLocation)
        {
            var client = GetClient();
            await client.DeleteObjectAsync(new DeleteObjectRequest() { BucketName = _privateSettings.StorageName, Key = fileName });
        }
    }
}