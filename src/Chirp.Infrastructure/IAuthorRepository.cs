namespace Chirp.Infrastructure;
using Core;

public interface IAuthorRepository
{
    public List<CheepDTO> AuthorCheep(int page, string? userName, string? selfemail);
    public void DeleteAuthor(string? email);
    public AuthorDTO GetAuthorDtoByName(string? name);
    public AuthorDTO GetAuthorDtoByEmail(string? email);
    public void ToggleFollow(string? selfemail, string? otheremail);
    public bool isFollowing(string? selfemail, string? otheremail);
    public bool isSelf(string? selfemail, string? otheremail);
    public List<AuthorDTO> GetFollowing(string? selfemail);
    public void ToggleBlocking(string? selfemail, string? otheremail);
    public bool isBlocking(string? selfemail, string? otheremail);
    public List<AuthorDTO> GetBlocking(string? selfemail);
}