using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using portfolio_business_logic;
using portfolio_website_models;

namespace portfolio_website.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly IRegister _register;
        private readonly IConfiguration _configuration;

        public RegisterController(IConfiguration _config, ILogger<RegisterController> logger, IRegister register)
        {
            this._logger = logger;
            this._register = register;
            this._configuration = _config;
        }

        public RegisterController()
        {
        }

        [HttpGet]
        [Route("testmethod/{num}")]
        public int testmethod(int num)
        {
            return 2;
        }

        /// <summary>
        /// This method will take a RegisterModel object and post a new account to the Database.
        /// It returns a RegisteredAccount Object
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("RegisterNewAccountAsync")]
        public async Task<ActionResult<int>> RegisterNewAccountAsync([FromBody] RegisterModel rm)
        {
            #region testing code for the Oracle Db 

            // string conString = "User Id=ADMIN;Password=123qwe123QWE;" +
            // //Set Data Source value to an Oracle net service name in
            // //  the tnsnames.ora file
            // "Data Source=marksportfoliodb_low;Connection Timeout=30;";

            // //Set the directory where the sqlnet.ora, tnsnames.ora, and 
            // //  wallet files are located
            // OracleConfiguration.TnsAdmin = @"D:\home\site\wwwroot\DB";
            // OracleConfiguration.WalletLocation = OracleConfiguration.TnsAdmin;

            // using (OracleConnection con = new OracleConnection(conString))
            // {
            //     using (OracleCommand cmd = con.CreateCommand())
            //     {
            //         cmd.CommandText = "INSERT INTO ACCOUNTS (username, password, hashedpassword, salutationid, firstname, lastname, occupationid, email, phonenumber, birthdate) VALUES(:username, :password, :hashedpassword, :salutationid, :firstname, :lastname, :occupationid, :email, :phonenumber, :birthdate)";
            //         cmd.Parameters.Add("username", rm.Username);
            //         cmd.Parameters.Add("password", rm.Password);
            //         cmd.Parameters.Add("hashedpassword", ".....");
            //         cmd.Parameters.Add("salutationId", rm.SalutationId);
            //         cmd.Parameters.Add("firstname", rm.FirstName);
            //         cmd.Parameters.Add("lastname", rm.LastName);
            //         cmd.Parameters.Add("occupationid", rm.OccupationId);
            //         cmd.Parameters.Add("email", rm.Email);
            //         cmd.Parameters.Add("phonenumber", rm.PhoneNumber);
            //         cmd.Parameters.Add("birthdate", rm.Birthdate);
            //         try
            //         {
            //             con.Open();
            //             int reader1 = 0;
            //             reader1 = await cmd.ExecuteNonQueryAsync(); // save the new user to the db.
            //             return reader1;
            //         }
            //         catch (Exception ex)
            //         {
            //             return BadRequest(ex.Source);
            //         }
            //     }
            // }
            #endregion

            if (!ModelState.IsValid) { return BadRequest(); }

            IDictionary<string, RegisteredAccount> ret = await _register.RegisterNewAccountAsync(rm);
            RegisteredAccount? ra;
            if (ret.TryGetValue("success", out ra))
                return Created($"http://localhost:5210/api/AccountInfo/{ra.AccountId}", ra);
            else
            {
                if (ret.TryGetValue("username", out ra))
                    return BadRequest("There is already an account with that username");
                //password
                else if (ret.TryGetValue("password", out ra))
                    return BadRequest("There is already an account with that password");
                //both
                else if (ret.TryGetValue("both", out ra))
                    return BadRequest("That username and password already exist.");
                else return BadRequest();
            }
        }

        /// <summary>
        /// TODO: get all the data about the logged in person.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("accountinfo/{accountId}")]
        public async Task<ActionResult<RegisteredAccount>> AccountInfo(int accountId)
        {
            if (!ModelState.IsValid) { return BadRequest(); }
            // TODO
            return new RegisteredAccount();
        }
    }
}