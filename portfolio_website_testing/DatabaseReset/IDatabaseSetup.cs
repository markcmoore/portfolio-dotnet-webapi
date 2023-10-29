using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace portfolio_website_testing.DatabaseReset
{
    public interface IDatabaseSetup
    {
        Task SetUpTestingDb(string testingDbConString);
    }
}