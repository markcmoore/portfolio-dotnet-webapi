using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using portfolio_business_logic;
using portfolio_website_models;
using portfolio_website_repo;
using portfolio_website_testing.DatabaseReset;
using Xunit;

namespace portfolio_website_testing
{
    public class Repository_testing
    {
        private readonly IConfiguration? _configuration = null;
        private string _TestingDbConStr = string.Empty;
        private readonly ILogger<Register_Repo_Access> _logger;
        private readonly IRegister_Repo_Access _repo;

        public Repository_testing()
        {
            this._configuration = new ConfigurationBuilder()
                .AddUserSecrets("d0e053c6-5dc4-4f19-88fd-b3768b30ce0e")
                .AddEnvironmentVariables()
                .Build();

            ILoggerFactory loggerFactory = LoggerFactory.Create(log => log.AddConsole());
            this._logger = loggerFactory.CreateLogger<Register_Repo_Access>();

            this._repo = new Register_Repo_Access(this._configuration, this._logger, "TestingDb");

            // TODO:haven't been able to get this to work with github actions
            this._TestingDbConStr = "Server=tcp:portfolio-website-server.database.windows.net,1433;Initial Catalog=portfolio-website-testing-db;Persist Security Info=False;User ID=portfolio-db;Password=marks1websiteDb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            // this._TestingDbConStr = this._configuration.GetConnectionString("TestingDb");
        }

        [Fact]
        public async Task SetUpAllTestTables()
        {
            DatabaseSetup dbSetup = new DatabaseSetup(this._TestingDbConStr);
            await dbSetup.DeleteExistingTablesIfexistAsync();
            await dbSetup.CreateTablesAsync();
            await dbSetup.PopulateSalutations();
            await dbSetup.PopulateOccupations();
            await dbSetup.PopulateTodos();
            await dbSetup.PopulateAccounts();
            await dbSetup.PopulateTodosJunction();
            Assert.True(true);
        }

        [Fact]
        public async Task RegisterNewAccountAsync_Returns1()
        {
            // ARRANGE - Create A Registermodel with the correct username and password. Create an instance of the Register_Repo_Access Class. 
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

    }
}