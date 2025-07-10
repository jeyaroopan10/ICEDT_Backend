using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using ICEDT.API.Services.Interfaces;
using ICEDT.API.DTO.Requst;
using ICEDT.API.DTO.Response;
using ICEDT.API.Middleware;
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

        public MediaService(IConfiguration configuration)
        {
            _configuration = configuration;

            // AWS Configuration
            var awsConfig = _configuration.GetSection("AWS");
            _bucketName = awsConfig["BucketName"];

            // Validate configuration
            var accessKey = awsConfig["AccessKey"];
            var secretKey = awsConfig["SecretKey"];
            var region = awsConfig["Region"];

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("AWS credentials are not configured properly");
            }

            // Create S3 client with explicit credentials
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
            };

            _s3Client = new AmazonS3Client(credentials, s3Config);
        }

        public async Task<MediaUploadResponseDto> UploadAsync(MediaUploadRequestDto request)
        {
            if (request.File == null || request.File.Length == 0)
                throw new BadRequestException("File is empty");

            // Validate file type and size (example: max 20MB)
            var allowedTypes = new[] { "image/jpeg", "image/png", "video/mp4", "audio/mpeg", "audio/mp3" };
            if (Array.IndexOf(allowedTypes, request.File.ContentType) < 0)
                throw new BadRequestException($"Invalid file type: {request.File.ContentType}");

            if (request.File.Length > 20 * 1024 * 1024)
                throw new BadRequestException("File size exceeds 20MB");

            // Generate unique key with folder structure
            var fileName = Path.GetFileName(request.File.FileName);
            var key = $"{request.Folder}/{Guid.NewGuid()}_{fileName}";

            try
            {
                using (var stream = request.File.OpenReadStream())
                {
                    var uploadRequest = new PutObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = key,
                        InputStream = stream,
                        ContentType = request.File.ContentType,
                        ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
                    };

                    var response = await _s3Client.PutObjectAsync(uploadRequest);

                    if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new InvalidOperationException("Failed to upload file to S3");
                    }
                }

                // Construct permanent public S3 URL
                var region = _configuration.GetSection("AWS")["Region"];
                var url = $"https://{_bucketName}.s3.{region}.amazonaws.com/{key}";

                return new MediaUploadResponseDto
                {
                    Key = key,
                    FileName = request.File.FileName,
                    Size = request.File.Length,
                    ContentType = request.File.ContentType,
                    Url = url,
                    Message = "File uploaded successfully"
                };
            }
            catch (AmazonS3Exception ex)
            {
                throw new InvalidOperationException($"S3 Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Upload Error: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new BadRequestException("Key cannot be empty");

            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    throw new InvalidOperationException("Failed to delete file from S3");
                }
            }
            catch (AmazonS3Exception ex)
            {
                throw new InvalidOperationException($"S3 Delete Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Delete Error: {ex.Message}", ex);
            }
        }

        public async Task<MediaListResponseDto> ListAsync(string folder)
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = string.IsNullOrEmpty(folder) ? "" : $"{folder}/",
                    MaxKeys = 1000
                };

                var response = await _s3Client.ListObjectsV2Async(request);
                var keys = new List<string>();

                foreach (var obj in response.S3Objects)
                {
                    keys.Add(obj.Key);
                }

                return new MediaListResponseDto
                {
                    Files = keys,
                    Count = keys.Count,
                    Folder = folder
                };
            }
            catch (AmazonS3Exception ex)
            {
                throw new InvalidOperationException($"S3 List Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"List Error: {ex.Message}", ex);
            }
        }

        public Task<MediaUrlResponseDto> GetPresignedUrlAsync(MediaUrlRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Key))
                throw new BadRequestException("Key cannot be empty");

            try
            {
                var presignedRequest = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = request.Key,
                    Expires = DateTime.UtcNow.AddMinutes(request.ExpiryMinutes),
                    Verb = HttpVerb.GET
                };

                var url = _s3Client.GetPreSignedURL(presignedRequest);
                
                return Task.FromResult(new MediaUrlResponseDto
                {
                    Url = url,
                    Key = request.Key,
                    ExpiryMinutes = request.ExpiryMinutes
                });
            }
            catch (AmazonS3Exception ex)
            {
                throw new InvalidOperationException($"S3 PreSigned URL Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"PreSigned URL Error: {ex.Message}", ex);
            }
        }

        public Task<string> GetPublicUrlAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new BadRequestException("Key cannot be empty");

            var region = _configuration.GetSection("AWS")["Region"];
            var url = $"https://{_bucketName}.s3.{region}.amazonaws.com/{key}";
            return Task.FromResult(url);
        }

        // Dispose method for cleanup
        public void Dispose()
        {
            _s3Client?.Dispose();
        }
    }
}