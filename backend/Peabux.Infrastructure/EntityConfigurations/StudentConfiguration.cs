using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Peabux.Domain.Entities;

namespace Peabux.Infrastructure.EntityConfigurations;
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.Property(m => m.UpdatedAt).IsRequired(false);
    }
}
