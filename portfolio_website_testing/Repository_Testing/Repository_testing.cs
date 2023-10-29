using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using portfolio_business_logic;
using portfolio_website_models;
using portfolio_website_repo;
using portfolio_website_testing.DatabaseReset;
using Xunit;
using Xunit.Sdk;

namespace portfolio_website_testing
{
    public class Repository_testing : IClassFixture<DatabaseSetup>// use the fixture to abstract away Db creation and seed.
    {
        private readonly IConfiguration? _configuration = null;
        private string _TestingDbConStr = string.Empty;
        private readonly ILogger<Register_Repo_Access> _logger;
        private readonly IRegister_Repo_Access _repo;
        private readonly DatabaseSetup _databaseSetup;

        public Repository_testing(DatabaseSetup databaseSetup)
        {
            this._configuration = new ConfigurationBuilder()
                // .AddUserSecrets("secrets.json")          // no worky               
                // .AddUserSecrets<Register_Repo_Access>(). // no worky
                .AddJsonFile("appsettings.json")            // WORKS!
                .AddEnvironmentVariables()
                .Build();
            // TODO: IDEA: 
            // 1)keep the testing con Str in github secrets. 
            // 2) during pipeline CI/CD, create a new 'appsettings.json' file at root append the con str in the {}"connectionStrings":{"TestingDb":"stringggggg"}" object (along with all settings needed for deployment).
            // OPTION: you may be able to make the file a different name to avoid any confusion.
            // 3) NOW the test suite can access it during CI/CD.

            //I'll need all these
            ILoggerFactory loggerFactory = LoggerFactory.Create(log => log.AddConsole());
            this._logger = loggerFactory.CreateLogger<Register_Repo_Access>();
            this._repo = new Register_Repo_Access(this._configuration, this._logger, "TestingDb");
            this._TestingDbConStr = "Server=tcp:portfolio-website-server.database.windows.net,1433;Initial Catalog=portfolio-website-testing-db;Persist Security Info=False;User ID=portfolio-db;Password=marks1websiteDb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            this._databaseSetup = databaseSetup;
        }

        // [Fact]
        private async Task SetUpTestingDb()
        {
            await this._databaseSetup.SetUpTestingDb(this._TestingDbConStr);
        }

        [Fact]
        public async Task RegisterNewAccountAsync_Returns1()
        {
            // ARRANGE - Create A Registermodel with the correct username and password.  
            Task t = this.SetUpTestingDb();
            RegisterModel rm = new RegisterModel()
            {
                Username = "default_value",
                Password = "default_value",
                SalutationId = 4,
                FirstName = "f4",
                LastName = "l4",
                Email = "e4@e4.com",
                OccupationId = 1,
                PhoneNumber = "1234567890",
                Birthdate = DateTime.Now
            };
            string hashedPassword = "hashed";
            await t;

            // ACT - INSERT the new entity.
            int ret = await this._repo.RegisterNewAccountAsync(rm, hashedPassword);

            //ASSERT - verify that the new entity was inserted successfully.
            Assert.True(ret == 1);
            // TODO: create Theories to test the limits of the data like stringlengths, etc.
        }

        [Fact]
        public async Task GetAccountByUsernameAndPassword_ReturnsRegisteredAccount()
        {
            // ARRANGE - Verify that the entity inserted in the previous test is there.
            string username = "default_value";
            string password = "default_value";

            // ACT - INSERT the new entity.
            RegisteredAccount ret = await this._repo.GetAccountByUsernameAndPassword(username, password);

            //ASSERT - verify that the new entity was inserted successfully.
            Assert.True(ret.Email == "e4@e4.com");
        }

        [Theory]
        [InlineData("Mark")]
        [InlineData("Moore")]
        [InlineData("Portfolio")]
        [InlineData("Website")]
        public void HashPassword_ReturnsHashedPassword(string password)
        {
            // ARRANGE
            PasswordHasher<RegisterModel>? ph = new PasswordHasher<RegisterModel>();
            string expected = ph.HashPassword(new RegisterModel(), password);
            // PasswordVerificationResult e1 = ph.VerifyHashedPassword();


            // ACT
            string actual = this._repo.HashPassword(new RegisterModel(), password);


            // ASSERT (expected, actual)
            Assert.True(ph.VerifyHashedPassword(new RegisterModel(), actual, password) == PasswordVerificationResult.Success);
        }
    }
}