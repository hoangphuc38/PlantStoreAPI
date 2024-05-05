using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;

namespace PlantStoreAPI.StaticServices
{
    public class UploadImage
    {
        private readonly Cloudinary _cloudinary;
        
        public UploadImage()
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            _cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            _cloudinary.Api.Secure = true;
        }

        public static UploadImage Instance = new UploadImage();

        public async Task<string> UploadAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file == null)
            {
                return string.Empty;
            }

            if (file.Length <= 0)
            {
                return string.Empty;
            }

            using (var stream = file.OpenReadStream())
            {                
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                                        .Height(400)
                                        .Width(400)
                                        .Crop("fill")
                                        .Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams); 
            }

            return uploadResult.Url.ToString();
        }
    }
}
