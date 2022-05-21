using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace YourVitebskWebServiceApp.Repositories
{
    public class ImageService
    {
        IWebHostEnvironment _appEnvironment;

        public ImageService(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public void SaveImages(string service, int id, IFormFileCollection uploadedFiles)
        {
            if (uploadedFiles.Count > 0)
            {
                string path = $"{_appEnvironment.WebRootPath}/images/{service}/{id}";
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                Directory.CreateDirectory(path);
                foreach (var file in uploadedFiles)
                {
                    string filePath = $"{path}/{file.FileName}";
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }
        }

        public void DeleteImages(string service, int id)
        {
            string path = $"{_appEnvironment.WebRootPath}/images/{service}/{id}";
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
