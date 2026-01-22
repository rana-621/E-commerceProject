using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
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


    public async Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams)
    {
        var query = _context.Products
            .Include(m => m.Category)
            .Include(m => m.photos)
            .AsNoTracking();

        //filtering by word(searching)

        if (!string.IsNullOrEmpty(productParams.Search))
        {
            //query = query.Where(m => m.Name.ToLower().Contains(productParams.Search.ToLower()) ||
            //                       m.Description.ToLower().Contains(productParams.Search.ToLower()));


            var searchWords = productParams.Search.Split(' ');
            query = query.Where(m => searchWords.All(word =>
            m.Name.ToLower().Contains(word.ToLower()) ||
            m.Description.ToLower().Contains(word.ToLower())
            ));
        }

        //Filter by category
        if (productParams.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == productParams.CategoryId);


        if (!string.IsNullOrEmpty(productParams.Sort))
        {
            query = productParams.Sort switch
            {
                "price_asc" => query.OrderBy(p => p.NewPrice),
                "price_desc" => query.OrderByDescending(p => p.NewPrice),
                _ => query.OrderBy(p => p.Name),
            };
        }

        query = query.Skip((productParams.PageNumber - 1) * productParams.pageSize).Take(productParams.pageSize);

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
