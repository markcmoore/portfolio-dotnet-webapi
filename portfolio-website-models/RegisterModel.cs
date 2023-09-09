using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace portfolio_website_models
{
    public class RegisterModel
    {
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
        public int SalutationId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Your first name must be between 1 and 50 characters. ")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Your last name must be between 1 and 50 characters. ")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Please use a valid email address. ")]
        public string Email { get; set; }

        public int OccupationId { get; set; }

        [Phone]
        public char[] PhoneNumber { get; set; }

    }
}