using Chirp.Core;
namespace Chirp.Infrastructure;


public interface ICheepService
{
    public List<CheepDTO> GetCheeps(int page);
    public List<CheepDTO> GetCheepsFromAuthor(string author , int page);
}