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
    public string AuthorId { get; set; }
    public Author Author { get; set; }
    public DateTime TimeStamp { get; set; }
}


public class Author : IdentityUser
{
    public ICollection<Cheep> Cheeps { get; set; }
}




