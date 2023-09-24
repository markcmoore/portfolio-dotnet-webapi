using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public RegisterController(ILogger<RegisterController> logger, IRegister register)
        {
            this._logger = logger;
            this._register = register;
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
        public async Task<ActionResult<RegisteredAccount>> RegisterNewAccountAsync([FromBody] RegisterModel rm)
        {
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