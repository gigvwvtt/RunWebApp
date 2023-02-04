using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RunWebApp.Helpers;
using RunWebApp.Interfaces;

namespace RunWebApp.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> options)
        {
            var acc = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if ( file.Length > 0) 
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicID)
        {
            var deleteParams = new DeletionParams(publicID);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}