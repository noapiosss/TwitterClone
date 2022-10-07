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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.Author)
            .HasForeignKey(u => u.AuthorUsername);
        
        modelBuilder.Entity<Like>().HasKey(nameof(Like.LikedPostId), nameof(Like.LikedByUsername));
        modelBuilder.Entity<Like>()
            .HasOne(l => l.LikedBy)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.LikedByUsername);
        modelBuilder.Entity<Like>()
            .HasOne(l => l.LikedPost)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.LikedPostId);        

        modelBuilder.Entity<Following>().HasKey(nameof(Following.FollowByUsername), nameof(Following.FollowForUsername));
        modelBuilder.Entity<Following>()
            .HasOne(f => f.FollowForUser)
            .WithMany(u => u.Followings)
            .HasForeignKey(f => f.FollowForUsername);
        modelBuilder.Entity<Following>()
            .HasOne(f => f.FollowByUser)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowByUsername);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
}