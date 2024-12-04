using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpDBContext : IdentityDbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    
    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Author>(entity =>
        {
            entity.HasIndex(a => a.Name).IsUnique();
        });
        
        // Configure the many-to-many relationship for Following
        builder.Entity("Chirp.Infrastructure.Author", b =>
        {
            b.HasOne("Chirp.Infrastructure.Author", null)
                .WithMany("Following")
                .HasForeignKey("AuthorId");
        });
        
        // Configure the many-to-many relationship for Blocking
        builder.Entity("Chirp.Infrastructure.Author", b =>
        {
            b.HasOne("Chirp.Infrastructure.Author", null)
                .WithMany("Blocking")
                .HasForeignKey("AuthorId");
        });

    }
    
}



