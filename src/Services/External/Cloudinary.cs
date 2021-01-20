using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Common.Helpers;
using Services.Contracts.External;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Services.External
{
    public class Cloudinary : ICloudinary
    {
        private readonly string cloudName;
        private readonly string apiKey;
        private readonly string apiSecret;

        public Cloudinary(string cloudName, string apiKey, string apiSecret)
        {
            this.cloudName = cloudName;
            this.apiKey = apiKey;
            this.apiSecret = apiSecret;
        }

        /// <summary>
        /// Uploads new image with selected parameters
        /// </summary>
        /// <param name="fileName">Name of the file to upload</param>
        /// <param name="imageMemoryStream">Memory stream containing the image data</param>
        /// <returns>Image uri / Error string, starting with "Error: "</returns>
        public async Task<string> UploadImageAsync(MemoryStream imageMemoryStream, string fileName)
        {
            imageMemoryStream.Position = 0;
            var cloudinaryAccount = new Account(cloudName, apiKey, apiSecret);
            var cloudinary = new CloudinaryDotNet.Cloudinary(cloudinaryAccount);
            string publicId = Guid.NewGuid().GenerateShortId() + fileName;
            var file = new FileDescription(fileName, imageMemoryStream);
            var uploadParams = new ImageUploadParams
            {
                File = file,
                Format = "png",
                PublicId = publicId,
                UseFilename = true,
            };
            uploadParams.Check();
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            if((uploadResult?.Error?.Message ?? "") != "")
            {
                return $"Error: {uploadResult.Error.Message}";
            }

            var uri = uploadResult.SecureUrl.AbsoluteUri;
            return uri;
        }
    }
}