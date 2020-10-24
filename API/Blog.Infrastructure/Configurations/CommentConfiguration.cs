using Blog.Domain.Models.Aggregates.Post;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable($"{nameof(Comment)}s");
            builder.Property(x => x.Id)
                .HasColumnName(nameof(Comment.Id))
                .IsRequired()
                .ValueGeneratedNever();

            builder.OwnsOne(x => x.Content, content =>
            {
                content.Property(y => y.Value)
                    .HasColumnName(nameof(Comment.Content))
                    .IsRequired();
            });

            builder.OwnsOne(x => x.Creator, creator =>
            {
                creator.Property(y => y.Value)
                    .HasColumnName(nameof(Creator))
                    .IsRequired();
            });
        }
    }
}