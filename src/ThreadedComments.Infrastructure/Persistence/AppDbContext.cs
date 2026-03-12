using DomainThread = ThreadedComments.Domain.Entities.Thread;
using Microsoft.EntityFrameworkCore;
using ThreadedComments.Domain.Entities;

namespace ThreadedComments.Infrastructure.Persistence;


public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<DomainThread> Threads => Set<DomainThread>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}