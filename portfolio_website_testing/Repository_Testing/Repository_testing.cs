using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Configuration;
using portfolio_website_testing.DatabaseReset;
using Xunit;

namespace portfolio_website_testing
{
    public class Repository_testing
    {
        private readonly IConfiguration? _configuration = null;
        private string? _TestingDbConStr = string.Empty;
        public Repository_testing()
        {
            this._configuration = new ConfigurationBuilder()
                .AddUserSecrets<Repository_testing>()
                .AddEnvironmentVariables()
                .Build();
            // this._TestingDbConStr = this._configuration.GetConnectionString("TestingDb");// TODO:haven't been able to get this to work with github actions
            this._TestingDbConStr = "Server=tcp:portfolio-website-server.database.windows.net,1433;Initial Catalog=portfolio-website-testing-db;Persist Security Info=False;User ID=portfolio-db;Password=marks1websiteDb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        [Fact]
        public async Task SetUpAllTestTables()
        {
            DatabaseSetup dbSetup = new DatabaseSetup(this._TestingDbConStr!);
            await dbSetup.DeleteExistingTablesIfexistAsync();
            await dbSetup.CreateTablesAsync();
            await dbSetup.PopulateSalutations();
            await dbSetup.PopulateOccupations();
            await dbSetup.PopulateTodos();
            await dbSetup.PopulateAccounts();
            await dbSetup.PopulateTodosJunction();


            Assert.True(true);
        }

    }
}