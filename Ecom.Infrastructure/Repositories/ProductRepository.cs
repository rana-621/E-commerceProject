using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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


    public async Task<IEnumerable<ProductDTO>> GetAllAsync(string sort)
    {
        var query = _context.Products
            .Include(m => m.Category)
            .Include(m => m.photos)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.NewPrice);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.NewPrice);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

        }

        var result = _mapper.Map<List<ProductDTO>>(query);
        return result;
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

    public async Task<bool> UpdateAsync(UpdateProductDTO updateproductDTO)
    {
        if (updateproductDTO is null)
        {
            return false;
        }
        var findProduct = await _context.Products.Include(m => m.Category)
            .Include(m => m.photos)
            .FirstOrDefaultAsync(p => p.Id == updateproductDTO.Id);
        if (findProduct is null)
        {
            return false;
        }
        _mapper.Map(updateproductDTO, findProduct);

        var findPhoto = await _context.Photos.Where(p => p.ProductId == updateproductDTO.Id).ToListAsync();
        _context.Photos.RemoveRange(findPhoto);
        foreach (var item in findPhoto)
        {
            _imageManagementService.DeleteImageAsync(item.ImageName);
        }
        _context.Photos.RemoveRange(findPhoto);

        var imagePath = await _imageManagementService.AddImageAsync(updateproductDTO.Photo, updateproductDTO.Name);
        var photo = imagePath.Select(path => new Photo
        {
            ImageName = path,
            ProductId = updateproductDTO.Id
        }).ToList();

        await _context.Photos.AddRangeAsync(photo);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(Product product)
    {
        var photo = await _context.Photos.Where(m => m.ProductId == product.Id).ToListAsync();
        foreach (var item in photo)
        {
            _imageManagementService.DeleteImageAsync(item.ImageName);
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

}
