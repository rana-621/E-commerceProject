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
    public Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
    {
        throw new NotImplementedException();
    }

    public Task DeleteImageAsync(string src)
    {
        throw new NotImplementedException();
    }
}
