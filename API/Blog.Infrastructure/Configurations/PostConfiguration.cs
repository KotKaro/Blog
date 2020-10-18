using Blog.Domain.Models.Aggregates.Post;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable($"{nameof(Post)}s");
            builder.Property(x => x.Id)
                .HasColumnName(nameof(Post.Id));

            builder.OwnsOne(x => x.Content, content =>
            {
                content.Property(y => y.Value)
                    .HasColumnName(nameof(Post.Content))
                    .IsRequired();
            });

            builder.OwnsOne(x => x.Title, title =>
            {
                title.Property(y => y.Value)
                    .HasColumnName(nameof(Post.Title))
                    .IsRequired();
            });
            builder.OwnsOne(x => x.CreationDate, creationDate =>
            {
                creationDate.Property(y => y.Value)
                    .HasColumnName(nameof(Post.CreationDate))
                    .IsRequired();
            });

        }
    }
}