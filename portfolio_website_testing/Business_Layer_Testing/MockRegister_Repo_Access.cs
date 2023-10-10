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
            throw new NotImplementedException();
        }

        public string HashPassword(RegisterModel x, string password)
        {
            var ph = new PasswordHasher<RegisterModel>();
            var ret = ph.HashPassword(x, password);
            return ret;
        }

        public async Task<RegisteredAccount> RegisterNewAccountAsync(RegisterModel rm, string hashedPassword)
        {
            RegisteredAccount ra = new RegisteredAccount() { Username = rm.Username, Password = rm.Password };

            // numbers 0,1,2,3, and 4 will return rm unchanged. failure will change the firstname prop to "failure"
            if (ra.Username.Equals("failure_case"))
            {
                ra.FirstName = "failure_case";
            }
            return ra;
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