using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.RenderTree;
using portfolio_website_testing.DatabaseReset;
using Xunit;

namespace portfolio_website_testing
{
    public class Repository_testing
    {
        [Fact]
        public async void CreateTablesAsync_SetsUpTableProofOfConcept()
        {
            // ARRANGE
            DatabaseSetup dbSetup = new DatabaseSetup();

            // ACT
            await dbSetup.CreateTablesAsync();

            //ASSERT
            Assert.True(false);
        }
    }
}