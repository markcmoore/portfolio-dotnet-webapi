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
// using portfolio_website_repo;
// using Microsoft.Extensions.Logging;

namespace portfolio_website_testing
{
    /// <summary>
    /// This class tests the Register controller and implements a mock Register Class instance and mock logger for DI.
    /// </summary>
    public class Controllers_testing
    {

        #region arrays to send to the different action methods.
        public static readonly IEnumerable<object[]> _RegisterNewAccountTestArray = new List<object[]>(){
            new object[] {new RegisterModel(){Username = "success"} },
            new object[] {new RegisterModel(){Username = "username"} },
            new object[] {new RegisterModel(){Username = "password"} },
            new object[] {new RegisterModel(){Username = "both"} }
        };

        #endregion
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

        [Theory]
        [MemberData(nameof(_RegisterNewAccountTestArray))]
        public async void RegisterNewAccountAsync_ReturnsRegisteredAccount(RegisterModel rm)
        {
            // ARRANGE - done above
            // ACT
            // get the actionresult containing the mocked return
            ActionResult<string> retDictionary = await this._registerController.RegisterNewAccountAsync(rm);

            // ASSERT
            if (retDictionary.Result is CreatedResult)
            {
                Assert.True(rm.Username == "success");
            }
            else if (retDictionary.Result is BadRequestObjectResult)
            {
                // test for the three possible returns based on the username sent in.
                if (rm.Username == "username")
                {
                    Assert.True(((retDictionary.Result as BadRequestObjectResult)?.Value as string) == "There is already an account with that username");
                }
                else if (rm.Username == "password")
                {
                    Assert.True(((retDictionary.Result as BadRequestObjectResult)?.Value as string) == "There is already an account with that password");
                }
                else if (rm.Username == "both")
                {
                    Assert.True(((retDictionary.Result as BadRequestObjectResult)?.Value as string) == "That username and password already exist.");
                }
                else Assert.Fail("A test case in RegisterNewAccountAsync_ReturnsRegisteredAccount failed");
            }
            else if (retDictionary.Result is BadRequestResult)
            {
                Assert.Fail("Something unexpected went wrong in RegisterNewAccountAsync_ReturnsRegisteredAccount");
            }
            else Assert.Fail("A test case in RegisterNewAccountAsync_ReturnsRegisteredAccount failed");
        }

        /// <summary>
        /// Complete - 
        /// </summary>
        /// <returns></returns>
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
            // ASSERT
            if (retRegisteredAccount.Result is OkObjectResult)
            {
                RegisteredAccount? ret = (retRegisteredAccount.Result as OkObjectResult)?.Value as RegisteredAccount;
                Assert.True(ret?.AccountId == mockAccount.AccountId);
            }
            else if (retRegisteredAccount.Result is BadRequestResult)
            {
                Assert.Fail("AccountInfo failed");
            }
        }

    }
}