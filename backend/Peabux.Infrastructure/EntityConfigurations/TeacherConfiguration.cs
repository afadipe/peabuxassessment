using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Peabux.Domain.Entities;

namespace Peabux.Infrastructure.EntityConfigurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.Property(m => m.UpdatedAt).IsRequired(false);
    }
}
