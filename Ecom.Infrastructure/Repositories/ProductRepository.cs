using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IImageManagementService _imageManagementService;
    public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
    {
        _context = context;
        _mapper = mapper;
        _imageManagementService = imageManagementService;
    }

    public async Task<bool> AddAsync(AddProductDTO productDTO)
    {
        if (productDTO == null)
            return false;

        var product = _mapper.Map<Product>(productDTO);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var imagePath = await _imageManagementService.AddImageAsync(productDTO.Photo, productDTO.Name);
        var photo = imagePath.Select(path => new Photo
        {
            ImageName = path,
            ProductId = product.Id
        }).ToList();
        await _context.Photos.AddRangeAsync(photo);
        await _context.SaveChangesAsync();
        return true;

    }
}
