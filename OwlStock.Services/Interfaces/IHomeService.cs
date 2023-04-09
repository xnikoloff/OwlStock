using OwlStock.Domain.Entities;

namespace OwlStock.Services.Interfaces
{
    public interface IHomeService
    {
        Task<string> ChooseHomePagePhoto(); 
    }
}
