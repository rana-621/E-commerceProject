using Ecom.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecom.Infrastructure.Data.Config;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasData(new Photo { Id = 3, ImageName = "test3", ProductId = 1 },
                        new Photo { Id = 4, ImageName = "test4", ProductId = 1 });
    }
}
