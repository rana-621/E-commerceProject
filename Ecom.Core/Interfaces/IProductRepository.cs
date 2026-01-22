using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;

namespace Ecom.Core.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<ProductDTO>> GetAllAsync(string sort, int? categoryId);
    Task<bool> AddAsync(AddProductDTO productDTO);
    Task<bool> UpdateAsync(UpdateProductDTO updateproductDTO);
    Task DeleteAsync(Product product);

}
