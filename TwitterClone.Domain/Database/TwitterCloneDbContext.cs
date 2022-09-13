using Microsoft.EntityFrameworkCore;
using TwitterClone.Contracts.Database;

namespace TwitterClone.Domain.Database;

public class TwitterCloneDbContext : DbContext
{  
    public DbSet<User> Users { get; init; }
    public DbSet<Post> Posts { get; init; }
    public DbSet<Following> Followings { get; init; }
    public DbSet<Like> Likes { get; set; }

    public TwitterCloneDbContext() : base()
    {
    }  
    public TwitterCloneDbContext(DbContextOptions<TwitterCloneDbContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=fyfnjksq123;Host=localhost;Port=5432;Database=twitterclonedb;Pooling=true;Min Pool Size=0;Max Pool Size=30;Connection Lifetime=0;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Like>().HasKey(nameof(Like.PostId), nameof(Like.Username));
        modelBuilder.Entity<Following>().HasKey(nameof(Following.FollowByUsername), nameof(Following.FollowForUsername));
    }
}