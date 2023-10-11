using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace portfolio_website_models
{
    public class TodoDetails
    {
        public int TodoId { get; set; } = -1;

        public int RegisteredAccountId { get; set; } = -1;

        [Required]
        public string TodoText { get; set; } = "default_value";

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}