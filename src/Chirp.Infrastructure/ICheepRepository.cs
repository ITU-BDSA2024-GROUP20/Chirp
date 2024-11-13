namespace Chirp.Infrastructure;

using Core;

public interface ICheepRepository
{
    public void CreateCheep(CheepDTO newCheep);
    public List<CheepDTO> ReadCheep( int page , string userName);
    public void UpdateCheep(CheepDTO alteredCheep);
    public Author GetAuthorByName(string name);
    public Author GetAuthorByEmail(string email);
    public void ToggleFollow(string self, CheepDTO cheep);
    public bool isFollowing(string self, CheepDTO cheep);
}