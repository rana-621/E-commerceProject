using System.ComponentModel.DataAnnotations.Schema;

namespace Ecom.Core.Entities.Product;

public class Product : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal NewPrice { get; set; }
    public decimal OldPrice { get; set; }
    public List<Photo> photos { get; set; } = new List<Photo>();
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]

    public virtual Category Category { get; set; } = null!;
}
