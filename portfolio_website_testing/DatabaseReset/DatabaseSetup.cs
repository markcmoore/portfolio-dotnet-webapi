using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace portfolio_website_testing.DatabaseReset
{
    public class DatabaseSetup : IDatabaseSetup
    {
        private string _TestingDbConStr = string.Empty;
        public DatabaseSetup(string conStr)
        {
            this._TestingDbConStr = conStr;
        }

        // private string TestingDbConStr { get; set; } = "Server=tcp:portfolio-website-server.database.windows.net,1433;Initial Catalog=portfolio-website-testing-db;Persist Security Info=False;User ID=portfolio-db;Password=marks1websiteDb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public async Task CreateTablesAsync()
        {
            string queryString = "DROP TABLE IF EXISTS Salutations;" +
            "CREATE TABLE Salutations(SalutationId INT PRIMARY KEY IDENTITY(1,1),Salutation VARCHAR(20) NOT NULL,CreatedOn DATETIME DEFAULT GETDATE())";

            using (SqlConnection con = new SqlConnection(this._TestingDbConStr))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    con.Open();
                    int reader1 = 0;
                    try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
                    catch (DbException ex)
                    {
                        Console.WriteLine($"There was an error in portfolio_website_testing.DatabaseReset.CreateTablesAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    // if (reader1 == 1){}
                    con.Close();
                }
            }
        }

        public Task DeleteExistingTablesIfexistAsync()
        {
            throw new NotImplementedException();
        }

        public Task PopulateAccounts()
        {
            throw new NotImplementedException();
        }

        public Task PopulateOccupations()
        {
            throw new NotImplementedException();
        }

        public Task PopulateSalutations()
        {
            throw new NotImplementedException();
        }

        public Task PopulateTodos()
        {
            throw new NotImplementedException();
        }

        public Task PopulateTodosJunction()
        {
            throw new NotImplementedException();
        }
    }
}