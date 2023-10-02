using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using portfolio_website_models;
using portfolio_website_repo;
using Microsoft.Extensions.Logging;


namespace portfolio_business_logic
{
    public class Register : IRegister
    {
        // private readonly ILogger<Register> _logger;
        private readonly IRegister_Repo_Access _repo;

        public Register()
        {
            this._repo = null;
        }

        public Register(IRegister_Repo_Access rra/*, ILogger<Register> logger**/)
        {
            this._repo = rra;
            // this._logger = logger;
        }

        /// <summary>
        /// This method will verify that the model data is within bounds, as necessary. 
        /// Create a hash of the given password and send the object to the repo layer for POST actions.
        /// returns a string indicating success or failure and a RegisteredAccoiunt object.
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, RegisteredAccount>> RegisterNewAccountAsync(RegisterModel rm)
        {
            // TODO: call bool method to make sure there isn't already a similar username and that the password is unique. 
            int exists = await this._repo.UserNameOrPasswordUsedAsync(rm.Username, rm.Password);
            IDictionary<string, RegisteredAccount> dict = new Dictionary<string, RegisteredAccount>();

            if (exists == 0)
            {
                string hashedPassword = this._repo.HashPassword(rm, rm.Password);// get the password hash.
                RegisteredAccount ret = await this._repo.RegisterNewAccountAsync(rm, hashedPassword);// insert into the Db.
                if (!ret.FirstName.Equals("failure"))
                {
                    dict.Add("success", ret);
                    return dict;
                }
            }
            else if (exists == 1)
            {
                string retStr = "username";
                dict.Add(retStr, new RegisteredAccount());
                return dict;
            }
            else if (exists == 2)
            {
                string retStr = "password";
                dict.Add(retStr, new RegisteredAccount());
                return dict;
            }
            else if (exists == 3)
            {
                string retStr = "both";
                dict.Add(retStr, new RegisteredAccount());
                return dict;
            }
            //no successfull outcome makes it this far, little guy. Try again.
            dict.Add("ERROR: There was a database error. Try again.", new RegisteredAccount());
            return dict;
        }
    }
}