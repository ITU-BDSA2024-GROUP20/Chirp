using System.ComponentModel.DataAnnotations;
namespace Chirp.Core;

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

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public ICollection<Cheep> Cheeps { get; set; }
    public string Email { get; set; }
}