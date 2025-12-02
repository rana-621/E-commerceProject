using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Infrastructure.Repositories.Service;

public class ImageManagementService : IImageManagementService
{
    private readonly IFileProvider _fileProvider;

    public ImageManagementService(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }
    public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
    {
        var SaveImagesSrc = new List<string>();
        var DirectoryImages = Path.Combine("wwwroot", "Images", src);
        if (Directory.Exists(DirectoryImages) is not true)
        {
            Directory.CreateDirectory(DirectoryImages);
        }

        foreach (var item in files)
        {
            if (item.Length > 0)
            {
                var ImageName = item.FileName;
                var ImageSrc = $"/Images/{src}/{ImageName}";
                var root = Path.Combine(DirectoryImages, ImageName);
                using (FileStream fileStream = new FileStream(root, FileMode.Create))
                {
                    await item.CopyToAsync(fileStream);

                }
                SaveImagesSrc.Add(ImageSrc);
            }
        }
        return SaveImagesSrc;

    }



    public void DeleteImageAsync(string src)
    {
        var info = _fileProvider.GetFileInfo(src);
        var root = info.PhysicalPath;
        File.Delete(info.PhysicalPath);
    }
}
