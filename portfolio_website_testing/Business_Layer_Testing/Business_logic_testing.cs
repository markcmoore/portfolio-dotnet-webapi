using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using portfolio_business_logic;
using portfolio_website_models;
using portfolio_website_repo;
using Xunit;

namespace portfolio_website_testing
{
    public class Business_logic_testing
    {
        // private readonly IConfiguration _configuration;
        // private readonly IRegister_Repo_Access _repo;
        private readonly ILogger<Register> _logger;
        private readonly IRegister_Repo_Access _mockregrepo;
        private readonly IRegister _registerClass;
        public Business_logic_testing()
        {
            this._mockregrepo = new MockRegister_Repo_Access();
            ILoggerFactory loggerFactory = LoggerFactory.Create(log => log.AddConsole());
            this._logger = loggerFactory.CreateLogger<Register>();
            this._registerClass = new Register(this._mockregrepo, this._logger);
        }


        // [Fact]
        // public void AccountInfo_ReturnsCorrectAccount()
        // {
        //     Assert.True(false);
        // }

        [Theory]
        [InlineData("zerozerozero", "zerozerozero")]
        [InlineData("oneoneoneone", "oneoneoneone")]
        [InlineData("twotwotwotwo", "twotwotwotwo")]
        [InlineData("threethree", "threethree")]
        [InlineData("failure_case", "failure_case")]
        public async void RegisterNewAccountAsync_ReturnsRegisteredAccountAndMessage(string username, string password)
        {
            // ARRANGE - create the Register Class instance
            // create a registeredaccount
            RegisterModel rm = new RegisterModel() { Username = username, Password = password };

            // ACT
            // creat dict to store the result of the username/password search and insert
            IDictionary<string, RegisteredAccount>? ret = await this._registerClass.RegisterNewAccountAsync(rm);

            // ASSERT
            if (username.Equals("oneoneoneone"))
            {
                Assert.True(ret.ContainsKey("username"));
            }
            if (username.Equals("twotwotwotwo"))
            {
                Assert.True(ret.ContainsKey("password"));
            }
            if (username.Equals("threethree"))
            {
                Assert.True(ret.ContainsKey("both"));
            }
            if (username.Equals("zerozerozero"))
            {
                Assert.True(ret.ContainsKey("success"));
            }
            if (username.Equals("failure_case"))
            {
                Assert.True(ret.ContainsKey("ERROR: There was a database error. Try again."));
                // Assert.True(ra.FirstName.Equals("failure"));
            }

        }

        // [Fact]
        // public void AccountInfo_ReturnsCorrectAccount()
        // {
        // ARRANGE


        // ACT


        // ASSERT
        //     Assert.True(false);
        // }

        // [Fact]
        // public void AccountInfo_ReturnsCorrectAccount()
        // {
        // ARRANGE


        // ACT


        // ASSERT
        //     Assert.True(false);
        // }

        // [Fact]
        // public void AccountInfo_ReturnsCorrectAccount()
        // {
        // ARRANGE


        // ACT


        // ASSERT
        //     Assert.True(false);
        // }


    }
}