using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace portfolio_website_models
{
    public class RegisteredAccount : RegisterModel
    {
        public RegisteredAccount() { }

        public RegisteredAccount(
            string UserName, string Password, string HashedPassword,
            int AccountId, int SalutationId, string Salutation, string FirstName, string LastName,
            string Email, int OccupationId, string Occupation, string PhoneNumber, DateTime Birthdate,
            DateTime CreatedOn
            ) : base(UserName, Password, SalutationId, FirstName, LastName, Email, OccupationId, PhoneNumber, Birthdate)
        {
            this.AccountId = AccountId;
            this.Salutation = Salutation;
            this.Occupation = Occupation;
            this.CreatedOn = CreatedOn;
            this.PasswordHash = HashedPassword;
        }

        public int AccountId { get; set; } = -1;
        public string Salutation { get; set; } = "default_value";
        public string Occupation { get; set; } = "default_value";
        public string PasswordHash { get; set; } = "default_value";
    }
}