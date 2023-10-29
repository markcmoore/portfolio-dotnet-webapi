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
        public DatabaseSetup() { }

        public async Task SetUpTestingDb(string testingDbConString)
        {
            this._TestingDbConStr = testingDbConString;
            await this.DeleteExistingTablesIfexistAsync();
            await this.CreateTablesAsync();
            await this.PopulateSalutations();
            await this.PopulateOccupations();
            await this.PopulateTodos();
            await this.PopulateAccounts();
            await this.PopulateTodosJunction();
        }

        private async Task DeleteExistingTablesIfexistAsync()
        {
            string queryString = "DROP TABLE IF EXISTS TodosJunction;DROP TABLE IF EXISTS Accounts;DROP TABLE IF EXISTS Todos;DROP TABLE IF EXISTS OCCUPATIONS;DROP TABLE IF EXISTS Salutations;";

            using (SqlConnection con = new SqlConnection(this._TestingDbConStr))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    con.Open();
                    int reader1 = 0;
                    try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
                    catch (DbException ex)
                    {
                        Console.WriteLine($"There was an error in portfolio_website_testing.DatabaseReset.DeleteExistingTablesIfexistAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    // if (reader1 == 1){}
                    con.Close();
                }
            }
        }

        private async Task CreateTablesAsync()
        {
            string queryString =
            "CREATE TABLE Salutations (SalutationId INT PRIMARY KEY IDENTITY(1,1),Salutation VARCHAR(20) NOT NULL,CreatedOn DATETIME DEFAULT GETDATE(),);" +
            //OCCUPATIONS
            "CREATE TABLE Occupations (OccupationId INT PRIMARY KEY IDENTITY(1,1),Occupation VARCHAR(20) NOT NULL,CreatedOn DATETIME DEFAULT GETDATE(),);" +
            //TODOS
            "CREATE TABLE Todos (TodoId INT PRIMARY KEY IDENTITY(1,1),Todo VARCHAR(255),CreatedOn DATETIME DEFAULT GETDATE(),);" +
            //ACCOUNTS
            "CREATE TABLE Accounts (AccountId INT PRIMARY KEY IDENTITY(1,1),Username VARCHAR(50) NOT NULL,Password VARCHAR(50) NOT NULL,HashedPassword VARCHAR(255) NOT NULL,SalutationId_FK INT FOREIGN KEY REFERENCES Salutations(SalutationId) On DELETE SET NULL,FirstName VARCHAR(50) NOT NULL,LastName VARCHAR(50) NOT NULL,OccupationId_FK INT REFERENCES OCCUPATIONS(OccupationId) On DELETE SET NULL,Email VARCHAR(50) NOT NULL,EmailConfirmed BIT DEFAULT 0,PhoneNumber CHAR(10) NOT NULL,PhoneConfirmed BIT DEFAULT 0,Birthdate DATE NOT NULL,CreatedOn DATETIME DEFAULT GETDATE(),HasSentEmail BIT DEFAULT 0,HasMadeOffer BIT DEFAULT 0);" +
            //TODOSJUNCTION
            "CREATE TABLE TodosJunction (JunctionId INT PRIMARY KEY IDENTITY(1,1),AccountId_FK INT FOREIGN KEY REFERENCES Accounts(AccountId) On DELETE SET NULL,TodoId_FK INT FOREIGN KEY REFERENCES Todos(TodoId) On DELETE SET NULL,CreatedOn DATETIME DEFAULT GETDATE(),);";

            using (SqlConnection con = new SqlConnection(this._TestingDbConStr))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    con.Open();
                    int reader1 = 0;
                    try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
                    catch (DbException ex)
                    {
                        Console.WriteLine($"There was an error in portfolio_website_testing.CreateTablesAsync.DeleteExistingTablesIfexistAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    // if (reader1 == 1){}
                    con.Close();
                }
            }
        }

        private async Task PopulateSalutations()
        {
            string queryString =
            "INSERT INTO Salutations (Salutation) VALUES ('Sir');" +
            "INSERT INTO Salutations (Salutation) VALUES ('Miss'); " +
            "INSERT INTO Salutations (Salutation) VALUES ('Mrs.'); " +
            "INSERT INTO Salutations (Salutation) VALUES ('Mr.'); " +
            "INSERT INTO Salutations (Salutation) VALUES ('Dr.'); " +
            "INSERT INTO Salutations (Salutation) VALUES ('Professor');" +
            "INSERT INTO Salutations (Salutation) VALUES ('Master');" +
            "INSERT INTO Salutations (Salutation) VALUES ('Ms.');" +
            "INSERT INTO Salutations (Salutation) VALUES ('Madam');" +
            "INSERT INTO Salutations (Salutation) VALUES ('Captain');" +
            "INSERT INTO Salutations (Salutation) VALUES ('Coach');" +
            "INSERT INTO Salutations (Salutation) VALUES ('Reverend');" +
            "INSERT INTO Salutations (Salutation) VALUES ('Your Honor');" +
            "INSERT INTO Salutations (Salutation) VALUES ('Your Highness');";

            using (SqlConnection con = new SqlConnection(this._TestingDbConStr))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    con.Open();
                    int reader1 = 0;
                    try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
                    catch (DbException ex)
                    {
                        Console.WriteLine($"There was an error in portfolio_website_testing.CreateTablesAsync.DeleteExistingTablesIfexistAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    // if (reader1 == 1){}
                    con.Close();
                }
            }            //
        }

        private async Task PopulateOccupations()
        {
            string queryString =
            "INSERT INTO OCCUPATIONS (Occupation) VALUES ('Recruiter');" +
            "INSERT INTO OCCUPATIONS (Occupation) VALUES ('Hiring Manager');" +
            "INSERT INTO OCCUPATIONS (Occupation) VALUES ('Other');" +
            "INSERT INTO OCCUPATIONS (Occupation) VALUES ('Friend');" +
            "INSERT INTO OCCUPATIONS (Occupation) VALUES ('Co-Worker');" +
            "INSERT INTO OCCUPATIONS (Occupation) VALUES ('Manager');" +
            "INSERT INTO OCCUPATIONS (Occupation) VALUES ('Subordinate');";

            using (SqlConnection con = new SqlConnection(this._TestingDbConStr))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    con.Open();
                    int reader1 = 0;
                    try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
                    catch (DbException ex)
                    {
                        Console.WriteLine($"There was an error in portfolio_website_testing.CreateTablesAsync.DeleteExistingTablesIfexistAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    // if (reader1 == 1){}
                    con.Close();
                }
            }
        }

        private async Task PopulateTodos()
        {
            string queryString =
            "INSERT INTO Todos (Todo) VALUES ('make a list');" +
            "INSERT INTO Todos (Todo) VALUES ('Do not make a list');" +
            "INSERT INTO Todos (Todo) VALUES ('Give mark a job');" +
            "INSERT INTO Todos (Todo) VALUES ('Compliment mark on his website.');";

            using (SqlConnection con = new SqlConnection(this._TestingDbConStr))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    con.Open();
                    int reader1 = 0;
                    try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
                    catch (DbException ex)
                    {
                        Console.WriteLine($"There was an error in portfolio_website_testing.CreateTablesAsync.DeleteExistingTablesIfexistAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    // if (reader1 == 1){}
                    con.Close();
                }
            }
        }

        private async Task PopulateAccounts()
        {
            string queryString =
            "INSERT INTO ACCOUNTS (USERNAME, PASSWORD, HASHEDPASSWORD, SALUTATIONID_FK, FIRSTNAME, LASTNAME, OCCUPATIONID_FK, EMAIL, EMAILCONFIRMED, PHONENUMBER, PHONECONFIRMED, BIRTHDATE, HASSENTEMAIL, HASMADEOFFER) VALUES ('a1', 'a1','a1hashed', 1, 'Mark','Moore',1,'mcm@h.com',1,'1234567890',1,'2011-11-07T19:01:55.714942+03:00',1,1);" +
            "INSERT INTO ACCOUNTS (USERNAME, PASSWORD, HASHEDPASSWORD, SALUTATIONID_FK, FIRSTNAME, LASTNAME, OCCUPATIONID_FK, EMAIL, EMAILCONFIRMED, PHONENUMBER, PHONECONFIRMED, BIRTHDATE, HASSENTEMAIL, HASMADEOFFER) VALUES ('a2', 'a2','a2hashed', 2, 'Arely','Garza-Moore',2,'abgr@gmail.com',0,'1234567890',0,'2017-09-08T19:01:55.714942+03:00',0,0);";

            using (SqlConnection con = new SqlConnection(this._TestingDbConStr))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    con.Open();
                    int reader1 = 0;
                    try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
                    catch (DbException ex)
                    {
                        Console.WriteLine($"There was an error in portfolio_website_testing.CreateTablesAsync.DeleteExistingTablesIfexistAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    // if (reader1 == 1){}
                    con.Close();
                }
            }
        }

        private async Task PopulateTodosJunction()
        {
            string queryString =
            "INSERT INTO TODOSJUNCTION (AccountId_FK, TodoId_FK) VALUES (1,1);" +
            "INSERT INTO TODOSJUNCTION (AccountId_FK, TodoId_FK) VALUES (1,2);" +
            "INSERT INTO TODOSJUNCTION (AccountId_FK, TodoId_FK) VALUES (2,3);" +
            "INSERT INTO TODOSJUNCTION (AccountId_FK, TodoId_FK) VALUES (2,4);";

            using (SqlConnection con = new SqlConnection(this._TestingDbConStr))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    con.Open();
                    int reader1 = 0;
                    try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
                    catch (DbException ex)
                    {
                        Console.WriteLine($"There was an error in portfolio_website_testing.CreateTablesAsync.DeleteExistingTablesIfexistAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    // if (reader1 == 1){}
                    con.Close();
                }
            }
        }
    }
}