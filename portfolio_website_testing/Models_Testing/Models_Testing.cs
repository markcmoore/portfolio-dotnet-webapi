using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using portfolio_website_models;
using Xunit;

namespace portfolio_website_testing.Models_Testing
{
    public class Models_Testing
    {
        [Fact]
        public void RegisteredAccount_ReturnsDefaultValues()
        {
            // ARRANGE - ACT
            RegisteredAccount regAcc = new RegisteredAccount();


            //ASSERT
            Assert.True(regAcc is RegisteredAccount);

        }

        [Fact]
        public void RegisterModel_ReturnsDefaultValues()
        {
            // ARRANGE - ACT
            RegisterModel regMod = new RegisterModel();


            //ASSERT
            Assert.True(regMod is RegisterModel);

        }


        [Fact]
        public void TodoDetails_ReturnsDefaultValues()
        {
            // ARRANGE - ACT
            TodoDetails todo = new TodoDetails();

            //ASSERT
            Assert.True(todo is TodoDetails);
        }
    }
}