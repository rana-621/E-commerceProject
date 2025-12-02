using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public ProductRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> AddAsync(AddProductDTO productDTO)
    {
        if (productDTO == null)
            return false;

        var product = _mapper.Map<Product>(productDTO);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }
}
