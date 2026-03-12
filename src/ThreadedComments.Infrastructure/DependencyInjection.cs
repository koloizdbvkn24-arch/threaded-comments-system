using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThreadedComments.Infrastructure.Factories;
using ThreadedComments.Infrastructure.Persistence.Repositories;
using ThreadedComments.Application.Interface.Repositories;
using ThreadedComments.Application.Interface.Factories;
using ThreadedComments.Infrastructure.Persistence;
using ThreadedComments.Application.Interface.Services;
using ThreadedComments.Application.Services;

namespace ThreadedComments.Infrastructure;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICommentFactory, CommentFactory>();
        services.AddScoped<IThreadRepository, ThreadRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}