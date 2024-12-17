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
        //insuring Name is unique
        builder.Entity<Author>(entity =>
        {
            entity.HasIndex(a => a.Name).IsUnique();
        });
        
        //ensures that author has a list of Authors name Following
        builder.Entity("Chirp.Infrastructure.Author", b =>
        {
            b.HasOne("Chirp.Infrastructure.Author", null)
                .WithMany("Following")
                .HasForeignKey("AuthorId");
        });
        
        //ensures that author has a list of Authors name Following
        builder.Entity("Chirp.Infrastructure.Author", b =>
        {
            b.HasOne("Chirp.Infrastructure.Author", null)
                .WithMany("Blocking")
                .HasForeignKey("AuthorId");
        });

    }
    
}



