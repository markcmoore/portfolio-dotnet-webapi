using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using portfolio_website_models;

namespace portfolio_website_repo
{
    public interface IRegister_Repo_Access
    {
        public Task<RegisteredAccount> RegisterNewAccountAsync(RegisterModel rm, string hashedPassword);
        public Task<List<TodoDetails>> Todos(int accountId);
        public Task<int> UserNameOrPasswordUsedAsync(string username, string password);
        public Task<RegisteredAccount> GetAccountByUsernameAndPassword(string userName, string password);
        public string HashPassword(RegisterModel x, string password);

    }
}