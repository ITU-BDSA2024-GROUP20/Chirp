using Microsoft.EntityFrameworkCore;
using Chirp.Core;
namespace Chirp.Infrastructure;




public class CheepService : ICheepService
{
    //private readonly DBFacade facade;
    private readonly ICheepRepository repository;
    
    public CheepService( ICheepRepository repository)
    {
        //this.facade = facade;
        this.repository = repository;
    }
	
    public List<CheepDTO> GetCheeps(int page)
    {
        return repository.ReadCheep(page, null);
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        // filter by the provided author name
        return repository.ReadCheep( page ,author);
    }
    

}