using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NuGet.Frameworks;
using portfolio_website_models;
using portfolio_website_repo;

namespace portfolio_website_testing
{
    public class MockRegister_Repo_Access : IRegister_Repo_Access
    {
        public async Task<RegisteredAccount> GetAccountByUsernameAndPassword(string userName, string password)
        {
            return new RegisteredAccount() { Username = userName, Password = password };
        }

        public string HashPassword(RegisterModel x, string password)
        {
            var ph = new PasswordHasher<RegisterModel>();
            var ret = ph.HashPassword(x, password);
            return ret;
        }

        public async Task<int> RegisterNewAccountAsync(RegisterModel rm, string hashedPassword)
        {
            RegisteredAccount ra = new RegisteredAccount() { Username = rm.Username, Password = rm.Password };

            // numbers 0,1,2,and 3 will return rm unchanged. 4 means a db error so it's a failure, so we will change the firstname property to "failure_case"
            if (ra.Username.Equals("failure_case"))
            {
                return 0;
            }
            return 1;
        }

        public async Task<List<TodoDetails>> Todos(int accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UserNameOrPasswordUsedAsync(string username, string password)
        {
            if (username == "oneoneoneone") return 1;
            else if (username == "twotwotwotwo") return 2;
            else if (username == "threethree") return 3;
            else return 0;
        }
    }
}