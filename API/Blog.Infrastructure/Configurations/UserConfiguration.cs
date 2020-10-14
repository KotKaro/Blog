using Blog.Domain.Models.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable($"{nameof(User)}s");
            builder.OwnsOne(x => x.UserDetails, content =>
            {
                content.Property(y => y.Username)
                    .IsRequired()
                    .HasColumnName(nameof(UserDetails.Username));
                content.Property(y => y.Password)
                    .IsRequired()
                    .HasColumnName(nameof(UserDetails.Password));
            });
        }
    }
}