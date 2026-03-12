using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Infrastructure.Persistence.Configuration;


public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ThreadId)
            .IsRequired();

        builder.Property(x => x.AuthorId)
            .IsRequired();

        builder.Property(x => x.ParentId)
            .IsRequired(false);

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdateAt)
            .IsRequired(false);

        builder.HasIndex(x => x.ThreadId);
        builder.HasIndex(x => x.ParentId);
        builder.HasIndex(x => x.AuthorId);
    }
}