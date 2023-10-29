using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
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
    [Trait("Category", "Controllers")]// this category will also run tests within the class that have a different category. use --> dotnet test --filter "Category = Controllers"
    public class Controllers_testing
    {
        #region Object arrays are sent to the different action methods.
        public static readonly IEnumerable<object[]> _RegisterNewAccountTestArray = new List<object[]>(){
            new object[] {new RegisterModel(){Username = "success"} },// each array element is an arg for the method. That is why it's an object array. 
            new object[] {new RegisterModel(){Username = "username"} },
            new object[] {new RegisterModel(){Username = "password"} },
            new object[] {new RegisterModel(){Username = "both"} }
        };
        #endregion

        private readonly ILogger<RegisterController> _logger;
        private readonly IRegister _register;
        private readonly RegisterController _registerController;
        public Controllers_testing()
        {
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
            Assert.Equal(1.62, 1.59, 1);//  1 rounds the numbers to the nearest 10th.
            Assert.Equal(1.629, 1.632, 2);//  1 rounds the numbers to the nearest 10th.

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
                Assert.True(rm.Username == "success");// TESTING: add a message for when the test fails.
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

        #region - trying out test synax from the tutorial https://www.udemy.com/course/unit-testing-net-core-2x-applications-with-xunit-net/
        [Fact]
        public void FactTestExamples()
        {
            //ints, doubles, etc.
            double dbl1 = 16.735, dbl2 = 16.68, dbl3 = 16.739;
            Assert.Equal(dbl1, dbl3, 0); // use 0 as the 3rd arg (precision) to compare to the nearest 10th. 
            Assert.Equal(dbl1, dbl2, 1); // use 1 as the 3rd arg to round to the nearest 10th.
            Assert.Equal(dbl1, dbl3, 1); // use 2 as the 3rd arg to round to the nearest 100th.

            //strings
            string tim1 = "tim", tim2 = "Tim";
            Assert.Equal(tim1, tim2, ignoreCase: true); // set ignore case to true to make the test case insensitive.
            Assert.Contains("ti", tim2, StringComparison.InvariantCultureIgnoreCase);// use the way to make the test case insensitive.
            Assert.StartsWith("ti", tim2, StringComparison.InvariantCultureIgnoreCase);
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", tim2 + " " + tim2);// use a regex to compare with. section explanation = [A-Z]{1} = the first letter must be capital in A-Z range, [a-z]+ = any amount of lower case letters. not the space. there must be a space between the words., [A-Z]{1}[a-z]+ = another word starting with a capital.
            string nullStr = null;
            Assert.Null(nullStr);
            string emptyStr = string.Empty;
            Assert.NotNull(emptyStr);
            Assert.True(string.IsNullOrEmpty(emptyStr));

            // Collections
            List<int> intList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            List<int> intList2 = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            Assert.All(intList, x => Assert.True(x > 0)); // .All() takes the collection and a predicate for how to assert on each value in the collection.
            Assert.Contains(11, intList); // iterate over the collection
            Assert.DoesNotContain(0, intList); // iterate over the collection
            Assert.Equal(intList2, intList);// see that 2 collections contain the same values, in the same order.

            // Ranges of values
            int age1 = 25, age2 = 30;
            Assert.InRange(age1, 18, age2);

            // checking that exceptions were thrown.
            Assert.Throws<ArgumentNullException>(() => Controllers_testing.ThrowsException());// make sure that the method throws an exception when you do a certain thing.
            var exceptionThrown = Assert.Throws<ArgumentNullException>(() => Controllers_testing.ThrowsException());// catch the exception that was thrown
            Assert.Equal("this is the test exception", exceptionThrown.ParamName);// check that the message sent with the exception is as expected.

            // check the type of an Object
            string str1 = "mark";
            Assert.IsType(typeof(string), str1);
            RegisteredAccount mockAccount = new RegisteredAccount()
            {
                AccountId = 212,
                CreatedOn = DateTime.Now,
                FirstName = "",
                LastName = "",
                Occupation = "test",
            };
            Assert.IsType(typeof(RegisteredAccount), mockAccount);
            RegisteredAccount ra = Assert.IsType<RegisteredAccount>(mockAccount);
            Assert.Equal(212, ra.AccountId);

        }

        // use the [Trait] attribute to group tests so that they are run together.
        [Fact]
        [Trait("Category", "Mark1")] // by giving the tests a category name, run only tests with that category name.
        public void DoAThin1g()
        {
            Assert.Equal(1, 1);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [Trait("Category", "Mark1")] // dotnet test --filter "Category=Mark1"
        public void DoAThing2(int a, bool b)
        {
            if (a != 2 && a != 3) Assert.Equal(a, 1);
            else Assert.True(2 == a || 3 == a);
        }

        private static void ThrowsException()
        {
            throw new ArgumentNullException("this is the test exception");
        }


        #endregion
    }// EoC
}// EoN