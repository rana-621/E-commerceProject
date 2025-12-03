using Microsoft.AspNetCore.Http;

namespace Ecom.Core.DTOs;

public record ProductDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public virtual List<PhotoDTO> Photos { get; set; } = new();
    public string CategoryName { get; set; } = string.Empty;

}
public record PhotoDTO
{
    public string ImageName { get; set; } = string.Empty;
    public int ProductId { get; set; }
}

public record AddProductDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal NewPrice { get; set; }
    public decimal OldPrice { get; set; }
    public int CategoryId { get; set; }

    public IFormFileCollection Photo { get; set; }
}