using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class Cheep
{
    public required int CheepId { get; set; }
    
    [Required]
    [StringLength(160)]
    public required string? Text { get; set; }
    [StringLength(128)]
    public required string AuthorId { get; set; }
    public required Author Author { get; set; }
    public required DateTime TimeStamp { get; set; }
}


public class Author : IdentityUser
{
    [StringLength(50)]
    public required string Name { get; set; }

    public required ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
    public required ICollection<Author> Following { get; set; } = new List<Author>();
    public required ICollection<Author> Blocking { get; set; } = new List<Author>(); }




