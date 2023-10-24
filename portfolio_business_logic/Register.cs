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
        private readonly IRegister_Repo_Access _repo;
        private readonly ILogger<Register> _logger;

        public Register(IRegister_Repo_Access rra, ILogger<Register> logger)
        {
            this._repo = rra;
            this._logger = logger;
        }

        public RegisteredAccount AccountInfo(int accountId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method will verify that the model data is within bounds, as necessary. 
        /// Create a hash of the given password and send the object to the repo layer for POST actions.
        /// Returns a string indicating success or failure and a RegisteredAccount object.
        /// </summary>
        /// <param name="rm"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, RegisteredAccount>> RegisterNewAccountAsync(RegisterModel rm)
        {
            // TODO: call bool method to make sure there isn't already a similar username and that the password is unique. 
            int exists = await this._repo.UserNameOrPasswordUsedAsync(rm.Username, rm.Password);
            // create a dictionary to hold the return from the next part.
            IDictionary<string, RegisteredAccount> dict = new Dictionary<string, RegisteredAccount>();

            if (exists == 0)
            {
                // get the password hash.
                string hashedPassword = this._repo.HashPassword(rm, rm.Password);
                int success = await this._repo.RegisterNewAccountAsync(rm, hashedPassword);
                if (success == 1)
                {
                    RegisteredAccount ret = await this._repo.GetAccountByUsernameAndPassword(rm.Username, rm.Password);
                    if (ret != null)
                    {
                        string retStr = "success";
                        dict.Add(retStr, ret);
                        return dict;
                    }
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
            //no successful outcome makes it this far, little guy. Try again.
            // this line serves to catch a db error in both the register or retreival of the new account
            dict.Add("ERROR: There was a database error. Try again.", new RegisteredAccount());
            return dict;
        }
    }
}