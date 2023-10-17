using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace portfolio_website_testing.DatabaseReset
{
    public interface IDatabaseSetup
    {
        Task DeleteExistingTablesIfexistAsync();
        Task CreateTablesAsync();
        Task PopulateSalutations();
        Task PopulateOccupations();
        Task PopulateTodos();
        Task PopulateAccounts();
        Task PopulateTodosJunction();
    }
}