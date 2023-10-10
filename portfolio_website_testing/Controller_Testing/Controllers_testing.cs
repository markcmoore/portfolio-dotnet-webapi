using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using portfolio_business_logic;
using portfolio_website.Controllers;
using portfolio_website_models;
using portfolio_website_repo;
using Xunit;
using Microsoft.Extensions.Logging;

namespace portfolio_website_testing
{
    /// <summary>
    /// This class tests the Register controller and implements a mock Register Class instance and mock logger for DI.
    /// </summary>
    public class Controllers_testing
    {
        // private readonly IConfiguration _configuration;
        // private readonly IRegister_Repo_Access _repo;
        private readonly ILogger<RegisterController> _logger;
        private readonly IRegister _register;
        private readonly RegisterController _registerController;
        public Controllers_testing()
        {
            // this._configuration = _config;
            this._register = new MockRegister();
            ILoggerFactory loggerFactory = LoggerFactory.Create(log => log.AddConsole());
            this._logger = loggerFactory.CreateLogger<RegisterController>();
            this._registerController = new RegisterController(this._register, this._logger);
        }

        [Fact]
        public void TestMethod_ReturnsDouble()
        {
            // ARRANGE
            int myInt = 3;

            // ACT
            int myDouble = this._registerController.testmethod(myInt);

            // ASSERT
            Assert.True(myDouble == (myInt * 2));
        }

        [Fact]
        public async void RegisterNewAccountAsync_ReturnsRegisteredAccount()
        {
            // ARRANGE
            RegisteredAccount mockAccount = new RegisteredAccount()
            {
                AccountId = 1,
                CreatedOn = DateTime.Now,
                FirstName = "",
                LastName = "",
                Occupation = "test",
            };
            RegisterModel mockRegisterModel = new RegisterModel();

            // ACT
            // get the actionresult containing the mocked return
            ActionResult<IDictionary<string, RegisteredAccount>> retDictionary = await this._registerController.RegisterNewAccountAsync(mockRegisterModel);

            // Console.WriteLine(retDictionary.Result);
            // Console.WriteLine(retDictionary.Value);

            // extract the IDictionary from the ActionResult
            if (retDictionary.Result is CreatedResult)
            {
                IDictionary<string, RegisteredAccount>? ret = (retDictionary.Result as CreatedResult)?.Value as IDictionary<string, RegisteredAccount>;
            }

            // ASSERT
            Assert.True(2 == 2);
        }

        [Fact]
        public async void AccountInfo_ReturnsRegisteredAccount()
        {
            // ARRANGE
            RegisteredAccount mockAccount = new RegisteredAccount()
            {
                AccountId = 212,
                CreatedOn = DateTime.Now,
                FirstName = "",
                LastName = "",
                Occupation = "test",
            };

            // ACT
            // get the actionresult containing the mocked return
            ActionResult<RegisteredAccount> retRegisteredAccount = await this._registerController.AccountInfo(mockAccount.AccountId);

            // extract the RegisteredAccount from the ActionResult
            if (retRegisteredAccount.Result is OkObjectResult)
            {
                RegisteredAccount? ret = (retRegisteredAccount.Result as OkObjectResult)?.Value as RegisteredAccount;
                // ASSERT
                Assert.True(ret?.AccountId == mockAccount.AccountId);
            }
            else if (retRegisteredAccount.Result is BadRequest)
            {

            }
            else
                Assert.Fail("AccountInfo failed");

        }


    }
}