using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ICEDT.API.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ICEDT.API.Services.Implementation
{
    public class MediaService : IMediaService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly IConfiguration _configuration;

        public MediaService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _configuration = configuration;
            _s3Client = s3Client;
            _bucketName = _configuration["AWS:BucketName"];
        }

        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            // Validate file type and size (example: max 20MB)
            var allowedTypes = new[] { "image/jpeg", "image/png", "video/mp4", "audio/mpeg", "audio/mp3" };
            if (Array.IndexOf(allowedTypes, file.ContentType) < 0)
                throw new ArgumentException("Invalid file type");
            if (file.Length > 20 * 1024 * 1024)
                throw new ArgumentException("File size exceeds 20MB");

            var key = $"{folder}/{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = key,
                    BucketName = _bucketName,
                    ContentType = file.ContentType
                };
                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);
            }
            return key;
        }

        public async Task DeleteAsync(string key)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };
            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        public async Task<IList<string>> ListAsync(string folder)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = folder + "/"
            };
            var response = await _s3Client.ListObjectsV2Async(request);
            var keys = new List<string>();
            foreach (var obj in response.S3Objects)
            {
                keys.Add(obj.Key);
            }
            return keys;
        }

        public Task<string> GetPresignedUrlAsync(string key, int expiryMinutes = 60)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes)
            };
            var url = _s3Client.GetPreSignedURL(request);
            return Task.FromResult(url);
        }
    }
} 