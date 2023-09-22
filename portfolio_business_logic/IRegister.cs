using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using portfolio_website_models;

namespace portfolio_business_logic
{
    public interface IRegister
    {
        public Task<IDictionary<string, RegisteredAccount>> RegisterNewAccountAsync(RegisterModel rm);
    }
}