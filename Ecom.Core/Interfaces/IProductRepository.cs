using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;
using Ecom.Core.Sharing;

namespace Ecom.Core.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams);
    Task<bool> AddAsync(AddProductDTO productDTO);
    Task<bool> UpdateAsync(UpdateProductDTO updateproductDTO);
    Task DeleteAsync(Product product);

}
