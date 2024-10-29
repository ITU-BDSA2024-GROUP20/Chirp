using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class Cheep
{
    public int CheepId { get; set; }
    
    [Required]
    [StringLength(160)]
    public string Text { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public DateTime TimeStamp { get; set; }
}

public class Author : IdentityUser<string>
{
    public string Name { get; set; }
    [Key]
    public int AuthorId { get; set; }
    public ICollection<Cheep> Cheeps { get; set; }
    public string Email { get; set; }
    
    public string Password { get; set; }
}




