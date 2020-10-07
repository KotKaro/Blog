using Blog.Domain.Models.Aggregates.Post;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.OwnsOne(x => x.Content, content =>
            {
                content.Property(x => x.Value).IsRequired();
            });

            builder.OwnsOne(x => x.Title, title =>
            {
                title.Property(x => x.Value).IsRequired();
            });
        }
    }
}