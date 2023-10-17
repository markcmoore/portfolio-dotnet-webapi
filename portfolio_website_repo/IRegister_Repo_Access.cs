using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using portfolio_website_models;

namespace portfolio_website_repo
{
    public interface IRegister_Repo_Access
    {
        Task<RegisteredAccount> RegisterNewAccountAsync(RegisterModel rm, string hashedPassword);
        Task<List<TodoDetails>> Todos(int accountId);
        Task<int> UserNameOrPasswordUsedAsync(string username, string password);
        Task<RegisteredAccount> GetAccountByUsernameAndPassword(string userName, string password);
        string HashPassword(RegisterModel x, string password);
    }
}