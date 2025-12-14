using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;

namespace Ecom.Core.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<bool> AddAsync(AddProductDTO productDTO);
    Task<bool> UpdateAsync(UpdateProductDTO updateproductDTO);

}
