using Project.Models;

namespace Project.Repository
{
    public interface IAccountRepository
    {
        Task<Account> CheckLogin(string email, string password);
        Task<Account> Register(Account account);
      

    }
}
