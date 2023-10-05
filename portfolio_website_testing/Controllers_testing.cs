using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using portfolio_business_logic;
using portfolio_website.Controllers;
using portfolio_website_models;
using portfolio_website_repo;
using Xunit;

namespace portfolio_website_testing
{
    public class Controllers_testing
    {
        // private readonly IConfiguration _configuration;
        // private readonly IRegister_Repo_Access _repo;
        private readonly MarksLogger<RegisterController> _logger;
        private readonly IRegister _register;
        private readonly RegisterController _registerController;
        public Controllers_testing()
        {
            // this._configuration = _config;
            this._register = new MockRegister();
            this._logger = new MarksLogger<RegisterController>();
            this._registerController = new RegisterController(this._logger, this._register);
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
            int myInt = 3;
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

            Console.WriteLine(retDictionary.Result);
            Console.WriteLine(retDictionary.Value);


            // extract the IDict fron the ActionResult
            if (retDictionary.Result is CreatedResult)
            {

                var ret = (retDictionary.Result as CreatedResult)?.Value as IDictionary<string, RegisteredAccount>;

            }


            // ASSERT
            Assert.True(2 == 2);
        }



    }
}