using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace portfolio_website_models
{
    public class RegisterModel
    {
        public RegisterModel() { }

        public RegisterModel(
            string UserName = "default_value",
            string Password = "default_value",
            int SalutationId = 0,
            string FirstName = "default_value",
            string LastName = "default_value",
            string Email = "default_value",
            int OccupationId = 0,
            string PhoneNumber = "default_value",
            DateTime Birthdate = new DateTime()
            )
        {
            this.Username = UserName;
            this.Password = Password;
            this.SalutationId = SalutationId;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.OccupationId = OccupationId;
            this.PhoneNumber = PhoneNumber;
            this.Birthdate = Birthdate;
        }

        //   AccountId INT IDENTITY(1,1),
        //   SalutationId FOREIGN KEY REFERENCES Salutations(SalutationId) On DELETE SET NULL,
        //   FirstName VARCHAR(50) NOT NULL,
        //   LastName VARCHAR(50) NOT NULL,
        //   Email VARCHAR(50) NOT NULL,
        //   EmailConfirmed BOOLEAN DEFAULT FALSE,
        //   OccupationId KEY REFERENCES OCCUPATIONS(OccupationId) On DELETE SET NULL,
        //   PhoneNumber CHAR(10), NOT NULL,
        //   PhoneComfirmed BOOLEAN DEFAULT FALSE,
        //   -- ProfileImage
        //   Birthdate DATE NULL,
        //   CreatedOn DATETIME DEFAULT GETDATE(),
        //   HasSentEmail BOOLEAN DEFAULT FALSE,
        //   HasMadeOffer BOOLEAN DEFAULT FALSE

        //no account ID yet.

        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Your username must be between 10 and 50 characters. ")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Your password must be between 10 and 50 characters. ")]
        public string Password { get; set; }

        public int SalutationId { get; set; } = 0;

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Your first name must be between 1 and 50 characters. ")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Your last name must be between 1 and 50 characters. ")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please use a valid email address. ")]
        public string Email { get; set; }

        public int OccupationId { get; set; }

        [Required]
        [Phone(ErrorMessage = "You must provide a phone number")]// this may require you to have a certain format when sending from the FE.
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Your last name must be between 1 and 50 characters. ")]
        public string PhoneNumber { get; set; }

        public DateTime Birthdate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}