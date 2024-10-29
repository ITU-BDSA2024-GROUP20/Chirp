namespace Chirp.Infrastructure;

using Core;

public interface ICheepRepository
{
    public void CreateCheep(CheepDTO newCheep);
    public List<CheepDTO> ReadCheep( int page , string userName);
    public void UpdateCheep(CheepDTO alteredCheep);
    public void CreateAuthor(string name, string email);
    public Author GetAuthorByName(string name);
    public Author GetAuthorByEmail(string email);
}