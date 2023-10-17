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
                .Build(); ;
            this._TestingDbConStr = this._configuration.GetConnectionString("TestingDb");
            Console.WriteLine(this._TestingDbConStr);
        }

        [Fact]
        public async void CreateTablesAsync_SetsUpTableProofOfConcept()
        {
            // ARRANGE
            DatabaseSetup dbSetup = new DatabaseSetup(this._TestingDbConStr!);

            // ACT
            await dbSetup.CreateTablesAsync();

            //ASSERT
            Assert.True(true);
        }
    }
}