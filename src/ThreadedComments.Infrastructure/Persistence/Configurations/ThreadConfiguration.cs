using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainThread = ThreadedComments.Domain.Entities.Thread;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Infrastructure.Persistence.Configuration;


public sealed class ThreadConfiguration : IEntityTypeConfiguration<DomainThread>
{
    public void Configure(EntityTypeBuilder<DomainThread> builder)
    {
        builder.ToTable("threads");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.CreadetAt)
            .IsRequired();

        builder.HasOne<Author>()
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.AuthorId);
    }
}