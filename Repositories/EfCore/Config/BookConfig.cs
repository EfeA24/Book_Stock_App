using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EfCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Books>
    {
        public void Configure(EntityTypeBuilder<Books> builder)
        {
            builder.HasData
                (
                    new Books { Id = 1, Name = "Anna Karenina", Price = 16 },
                    new Books { Id = 2, Name = "The Miserables", Price = 9 },
                    new Books { Id = 3, Name = "Crime and Punishment", Price = 18 }
                );
        }
    }
}
