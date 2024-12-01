namespace Chirp.Infrastructure;
using Core;

public interface IAuthorRepository
{
    public List<CheepDTO> AuthorCheep(int page, string userName, string self);
    public void DeleteAuthor(string username);
    public AuthorDTO GetAuthorDtoByName(string name);
    public AuthorDTO GetAuthorDtoByEmail(string email);
    public void ToggleFollow(string self, string other);
    public bool isFollowing(string self, string other);
    public bool isSelf(string self, string other);
    public List<AuthorDTO> GetFollowing(string self);
}