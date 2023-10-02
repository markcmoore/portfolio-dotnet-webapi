using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using portfolio_business_logic;
using portfolio_website.Controllers;
using portfolio_website_repo;
using Xunit;

namespace portfolio_website_testing
{
    public class Controllers_testing
    {
        // private readonly ILogger<RegisterController> _logger;
        // private readonly IRegister _register;
        // private readonly IConfiguration _configuration;

        // private readonly IRegister_Repo_Access _repo;
        // public Controllers_testing(IConfiguration _config, ILogger<RegisterController> logger, IRegister register, IRegister_Repo_Access repo)
        // {
        //     this._configuration = _config;
        //     this._logger = logger;
        //     this._register = register;
        // }

        [Fact]
        public void TestMethodReturns2()
        {
            // var _logger = new MarksLogger<RegisterController>();//  this is NOT correct
            // IRegister_Repo_Access rra = new Register_Repo_Access();
            IRegister registerClass = new Register();

            RegisterController rc = new RegisterController(/**_logger,**/ registerClass);
            int myInt = 3;
            int myDouble = rc.testmethod(myInt);
            Assert.True(myDouble == (myInt * 2));
        }
    }
}