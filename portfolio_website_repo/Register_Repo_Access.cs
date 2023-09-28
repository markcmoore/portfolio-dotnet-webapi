using System.Configuration;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using portfolio_website_models;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace portfolio_website_repo
{
    public class Register_Repo_Access : IRegister_Repo_Access, IRepoStringConfig
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Register_Repo_Access> _logger;
        public Register_Repo_Access(IConfiguration _config, ILogger<Register_Repo_Access> logger)
        {
            this._configuration = _config;
            this._logger = logger;
        }

        /// <summary>
        /// Takes a RegisterModel object and a has representation of the models password. 
        /// Inserts them into the Db.
        /// returns a RegisteredAccount object. If there was a Db INSERT faillure, the FirstName property is "failure".
        /// Otherwise, returns the registerd user who matches the username/password/hash combination.
        /// </summary>
        /// <param name="rm"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        public async Task<RegisteredAccount> RegisterNewAccountAsync(RegisterModel rm, string hashedPassword)
        {
            string queryString1 = $"INSERT INTO ACCOUNTS (username, password, hashedpassword, salutationid_FK, firstname, lastname, occupationid_FK, email, phonenumber, birthdate) VALUES(@username,@password,@hashedpassword,@salutationid,@firstname,@lastname,@occupationid,@email,@phonenumber,@birthdate)";

            using (SqlConnection con = new SqlConnection(this.GetConnectionString("AzureDb")))
            {

                using (SqlCommand cmd = new SqlCommand(queryString1, con))
                {

                    cmd.CommandText = queryString1;
                    cmd.Parameters.AddWithValue("username", rm.Username);
                    cmd.Parameters.AddWithValue("password", rm.Password);
                    cmd.Parameters.AddWithValue("hashedpassword", hashedPassword);
                    cmd.Parameters.AddWithValue("salutationId", rm.SalutationId);
                    cmd.Parameters.AddWithValue("firstname", rm.FirstName);
                    cmd.Parameters.AddWithValue("lastname", rm.LastName);
                    cmd.Parameters.AddWithValue("occupationid", rm.OccupationId);
                    cmd.Parameters.AddWithValue("email", rm.Email);
                    cmd.Parameters.AddWithValue("phonenumber", rm.PhoneNumber);
                    cmd.Parameters.AddWithValue("birthdate", rm.Birthdate);
                    con.Open();

                    int reader1 = 0;
                    try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
                    catch (DbException ex)
                    {
                        Console.WriteLine($"There was an error in Register_Repo_Access.RegisterNewAccountAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    if (reader1 == 1)
                    {
                        return await this.GetAccountByUsernameAndPassword(rm.Username, rm.Password);
                    }
                    else return new RegisteredAccount() { FirstName = "failure" };
                }
            }


        }

        /// <summary>
        /// Gets all the todos under a account id.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<TodoDetails>> Todos(int accountId)
        {
            OracleConnection con1 = new OracleConnection(this.GetConnectionString("AzureDb"));
            OracleCommand cmd1 = con1.CreateCommand();
            string querystring1 = "Select * from Todos";// TODO:create the big multi-join query string
            cmd1.CommandText = querystring1;
            con1.Open();
            DbDataReader reader1 = await cmd1.ExecuteReaderAsync();
            List<TodoDetails> todos = new List<TodoDetails>();
            if (reader1.Read())
            {
                return new List<TodoDetails>();
            }
            else return todos;
        }

        /// <summary>
        /// Queries the database for an account matching the username and password provided. 
        /// Returns relevant account data for display
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns> 
        /// <summary>
        public async Task<RegisteredAccount> GetAccountByUsernameAndPassword(string userName, string password)
        {
            string queryString = "Select a.AccountId, a.Username, a.Password, s.Salutation, a.FirstName, a.LastName, a.Email, a.Phonenumber, o.Occupation, a.Birthdate, a.CreatedOn FROM Accounts a LEFT JOIN Salutations s ON a.SalutationId_FK = s.SalutationId LEFT JOIN Occupations o ON a.OccupationId_FK = o.OccupationId WHERE a.username = @username AND a.password = @password";
            //string queryString1 = $"INSERT INTO ACCOUNTS (username, password, hashedpassword, salutationid, firstname, lastname, occupationid, email, phonenumber, birthdate) VALUES(@username,@password,@hashedpassword,@salutationid,@firstname,@lastname,@occupationid,@email,@phonenumber,@birthdate)";

            using (SqlConnection con = new SqlConnection(this.GetConnectionString("AzureDb")))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    cmd.CommandText = queryString;
                    cmd.Parameters.AddWithValue("@username", userName);
                    cmd.Parameters.AddWithValue("@password", password);

                    try
                    {
                        con.Open();
                        DbDataReader reader = await cmd.ExecuteReaderAsync();

                        if (await reader.ReadAsync())
                        {
                            RegisteredAccount newAcc = new RegisteredAccount()
                            {
                                AccountId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                Salutation = reader.GetString(3),
                                FirstName = reader.GetString(4),
                                LastName = reader.GetString(5),
                                Email = reader.GetString(6),
                                PhoneNumber = reader.GetString(7),
                                Occupation = reader.GetString(8),
                                Birthdate = reader.GetDateTime(9),
                                CreatedOn = reader.GetDateTime(10),
                            };
                            return newAcc;
                        }
                        else return null;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"There was an error in Register_Repo_Access.GetAccountByUsernameAndPassword - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    return null;
                }
            }
        }

        public string HashPassword(RegisterModel x, string password)
        {
            // dotnet add package Microsoft.Extensions.Identity.Core --version 7.0.11
            var ph = new PasswordHasher<RegisterModel>();
            var ret = ph.HashPassword(x, password);
            return ret;
        }

        /// <summary>
        /// Retrieves the correct connection string from the global context depending on Dev or Prod environment.
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public string GetConnectionString(string connectionName)
        {
            return this._configuration.GetConnectionString("AzureDb")!;
        }

        /// <summary>
        /// Checks the database for the existance of that username OR password.
        /// returns an int. 
        /// If there is at least 1 alike password in the db and 0 alike usernames, return 1,
        /// If there is 0 alike passwords in the db and 1 alike username, return 2,
        /// If there is at least 1 alike password and 1 alike username, return 3,
        /// If there was a problem with the Db, return 4,
        /// otherwise returns 0;
        /// /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<int> UserNameOrPasswordUsedAsync(string username, string password)
        {
            string queryString = "SELECT aa.usernames, bb.passwords FROM (SELECT COUNT(Username) AS usernames FROM Accounts WHERE username = @uname) aa, (SELECT COUNT(Password) AS passwords FROM Accounts WHERE password = @pword) bb";
            using (SqlConnection con = new SqlConnection(this.GetConnectionString("AzureDb")))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    cmd.CommandText = queryString;
                    cmd.Parameters.AddWithValue("@uname", username);
                    cmd.Parameters.AddWithValue("@pword", password);

                    try
                    {
                        con.Open();
                        DbDataReader reader1 = await cmd.ExecuteReaderAsync();

                        if (await reader1.ReadAsync())
                        {
                            if (reader1.GetInt32(0) == 0 && reader1.GetInt32(1) == 0)// if both username and password are available
                            {
                                return 0;
                            }
                            else if (reader1.GetInt32(0) > 0 && reader1.GetInt32(1) == 0)// if username is taken but password is available
                            {
                                return 1;
                            }
                            else if (reader1.GetInt32(0) == 0 && reader1.GetInt32(1) > 0)// if username is available but password is taken
                            {
                                return 2;
                            }
                            else return 3; //if both are taken
                        }
                        return 4;// error with the db.
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"There was an error in Register_Repo_Access.UserNameOrPasswordUsedAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
                    }
                    return 0;
                }
            }
        }

        #region Oracle Db access code.
        // /// <summary>
        // /// takes a registermodel object and a has representation of the models password. 
        // /// Inserts them into the Db.
        // /// returns a RegisteredAccount object. If there was a Db INSERT faillure, the FirstName property is "failure".
        // /// Otherwise, returns the registerd user who matches the username/password/hash combination.
        // /// </summary>
        // /// <param name="rm"></param>
        // /// <param name="hashedPassword"></param>
        // /// <returns></returns>
        // public async Task<RegisteredAccount> RegisterNewAccountAsync(RegisterModel rm, string hashedPassword)
        // {
        //     OracleConnection con = new OracleConnection(this.GetConnectionString("OracleDb"));
        //     OracleCommand cmd = con.CreateCommand();

        //     string querystring1 = $"INSERT INTO ACCOUNTS (username, password, hashedpassword, salutationid, firstname, lastname, occupationid, email, phonenumber, birthdate) VALUES(:username, :password, :hashedpassword, :salutationid, :firstname, :lastname, :occupationid, :email, :phonenumber, :birthdate)";
        //     cmd.CommandText = querystring1;

        //     cmd.Parameters.Add("username", rm.Username);
        //     cmd.Parameters.Add("password", rm.Password);
        //     cmd.Parameters.Add("hashedpassword", hashedPassword);
        //     cmd.Parameters.Add("salutationId", rm.SalutationId);
        //     cmd.Parameters.Add("firstname", rm.FirstName);
        //     cmd.Parameters.Add("lastname", rm.LastName);
        //     cmd.Parameters.Add("occupationid", rm.OccupationId);
        //     cmd.Parameters.Add("email", rm.Email);
        //     cmd.Parameters.Add("phonenumber", rm.PhoneNumber);
        //     cmd.Parameters.Add("birthdate", rm.Birthdate);

        //     con.Open();
        //     int reader1 = 0;
        //     try { reader1 = await cmd.ExecuteNonQueryAsync(); }// save the new user to the db.
        //     catch (DbException ex)
        //     {
        //         Console.WriteLine($"There was an error in Register_Repo_Access.RegisterNewAccountAsync - {ex.ErrorCode} - {ex.InnerException} - {ex.Message}");
        //     }
        //     if (reader1 == 1)
        //     {
        //         return await this.GetAccountByUsernameAndPassword(rm.Username, rm.Password);
        //     }
        //     else return new RegisteredAccount() { FirstName = "failure" };
        // }

        // /// <summary>
        // /// Gets all the todos under a account id.
        // /// </summary>
        // /// <param name="accountId"></param>
        // /// <returns></returns>
        // public async Task<List<TodoDetails>> Todos(int accountId)
        // {
        //     OracleConnection con1 = new OracleConnection(this.GetConnectionString("OracleDb"));
        //     OracleCommand cmd1 = con1.CreateCommand();
        //     string querystring1 = "Select * from Todos";// TODO:create the big multi-join query string
        //     cmd1.CommandText = querystring1;
        //     con1.Open();
        //     DbDataReader reader1 = await cmd1.ExecuteReaderAsync();
        //     List<TodoDetails> todos = new List<TodoDetails>();
        //     if (reader1.Read())
        //     {
        //         return new List<TodoDetails>();
        //     }
        //     else return todos;
        // }

        // /// <summary>
        // /// Queries the database for an account matching the username and password provided. 
        // /// Returns relevant account data for display
        // /// </summary>
        // /// <param name="userName"></param>
        // /// <param name="password"></param>
        // /// <returns></returns> 
        // /// <summary>
        // public async Task<RegisteredAccount> GetAccountByUsernameAndPassword(string userName, string password)
        // {
        //     OracleConnection con1 = new OracleConnection(this.GetConnectionString("OracleDb"));
        //     OracleCommand cmd1 = con1.CreateCommand();

        //     string querystring1 = "Select a.AccountId, a.Username, a.Password, s.Salutation, a.FirstName, a.LastName, a.Email, a.Phonenumber, o.Occupation, a.Birthdate, a.CreatedOn FROM Accounts a LEfT JOIN Salutations s ON a.SalutationId = s.SalutationId LEFT JOIN Occupations o ON a.OccupationId = o.OccupationId WHERE a.username = :username AND a.password = :password";

        //     cmd1.CommandText = querystring1;
        //     cmd1.Parameters.Add("username", userName);
        //     cmd1.Parameters.Add("password", password);

        //     con1.Open();
        //     DbDataReader reader1 = await cmd1.ExecuteReaderAsync();

        //     if (await reader1.ReadAsync())
        //     {
        //         RegisteredAccount newAcc = new RegisteredAccount()
        //         {
        //             AccountId = reader1.GetInt32(0),
        //             Username = reader1.GetString(1),
        //             Password = reader1.GetString(2),
        //             Salutation = reader1.GetString(3),
        //             FirstName = reader1.GetString(4),
        //             LastName = reader1.GetString(5),
        //             Email = reader1.GetString(6),
        //             PhoneNumber = reader1.GetString(7),
        //             Occupation = reader1.GetString(8),
        //             Birthdate = reader1.GetDateTime(9),
        //             CreatedOn = reader1.GetDateTime(10),
        //         };
        //         return newAcc;

        //     }
        //     else return null;
        // }

        // public string HashPassword(RegisterModel x, string password)
        // {
        //     // dotnet add package Microsoft.Extensions.Identity.Core --version 7.0.11
        //     var ph = new PasswordHasher<RegisterModel>();
        //     var ret = ph.HashPassword(x, password);
        //     return ret;
        // }

        // /// <summary>
        // /// Retrieves the correct connection string from the global context depending on Dev or Prod environment.
        // /// </summary>
        // /// <param name="connectionName"></param>
        // /// <returns></returns>
        // public string GetConnectionString(string connectionName)
        // {
        //     return this._configuration.GetConnectionString("OracleDb")!;
        // }

        // /// <summary>
        // /// Checks the database for the existance of that username OR password.
        // /// returns an int. 
        // /// If there is at least 1 alike password in the db and 0 alike usernames, return 1,
        // /// If there is 0 alike passwords in the db abd 1 alike username, return 2,
        // /// If there is at least 1 alike password and 1 alike username, return 3,
        // /// If there was a problem with the Db, return 4,
        // /// otherwise returns 0;
        // /// /// </summary>
        // /// <param name="username"></param>
        // /// <param name="password"></param>
        // /// <returns></returns>
        // public async Task<int> UserNameOrPasswordUsedAsync(string username, string password)
        // {
        //     OracleConnection con1 = new OracleConnection(this.GetConnectionString("OracleDb"));
        //     OracleCommand cmd1 = con1.CreateCommand();

        //     string querystring1 =
        //     "SELECT * FROM (SELECT COUNT(Username) AS usernames FROM Accounts WHERE username = :uname)," +
        //     " (SELECT COUNT(Password) AS passwords FROM Accounts WHERE password = :pword)";

        //     cmd1.CommandText = querystring1;
        //     cmd1.Parameters.Add("uname", username);
        //     cmd1.Parameters.Add("pword", password);

        //     con1.Open();
        //     DbDataReader reader1 = await cmd1.ExecuteReaderAsync();

        //     if (await reader1.ReadAsync())
        //     {
        //         if (reader1.GetInt32(0) == 0 && reader1.GetInt32(1) == 0)
        //         {
        //             return 0;
        //         }
        //         else if (reader1.GetInt32(0) > 0 && reader1.GetInt32(1) == 0)
        //         {
        //             return 1;
        //         }
        //         else if (reader1.GetInt32(0) == 0 && reader1.GetInt32(1) > 0)
        //         {
        //             return 2;
        //         }
        //         else return 3;
        //     }
        //     return 4;
        // }
        #endregion
    }//EoC
}//EoN