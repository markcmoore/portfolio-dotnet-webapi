using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using portfolio_business_logic;
using portfolio_website_models;

namespace portfolio_website_testing
{
    public class MockRegister : IRegister
    {

        public async Task<IDictionary<string, RegisteredAccount>> RegisterNewAccountAsync(RegisterModel rm)
        {
            RegisteredAccount mockAccount = new RegisteredAccount()
            {
                AccountId = Int32.MaxValue,
                CreatedOn = DateTime.Now,
                FirstName = "",
                LastName = "",
                Occupation = "test",
            };

            IDictionary<string, RegisteredAccount> mockDictionary = new Dictionary<string, RegisteredAccount>();
            mockDictionary.Add("success", mockAccount);
            return mockDictionary;
        }
    }
}