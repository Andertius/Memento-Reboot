using Memento.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Memento.Auth.Database.EntityConfigurations;

public sealed class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Token).IsRequired().HasMaxLength(256);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}
