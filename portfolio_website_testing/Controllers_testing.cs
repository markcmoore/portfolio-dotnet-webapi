using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using portfolio_website.Controllers;
using Xunit;

namespace portfolio_website_testing
{
    public class Controllers_testing
    {
        [Fact]
        public void TestMethodReturns2()
        {
            RegisterController rc = new RegisterController();
            int two = rc.testmethod(3);
            Assert.True(two == 2);
        }
    }
}