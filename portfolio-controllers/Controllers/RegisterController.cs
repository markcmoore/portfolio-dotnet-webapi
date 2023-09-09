using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace portfolio_website.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger) { _logger = logger; }

        [HttpGet(Name = "register")]
        public string Register()
        {
            // string ConString3 = "User Id=ADMIN;Password=123qwe123QWE;Data Source=(description= (retry_count=20)(retry_delay=3)(address=(protocol=tcps)(port=1521)(host=adb.us-chicago-1.oraclecloud.com))(connect_data=(service_name=gf86a9662bf3953_marksportfoliodb_high.adb.oraclecloud.com))(security=(ssl_server_dn_match=yes)))";
            // OracleConnection con1 = new OracleConnection(ConString3);
            // OracleCommand cmd1 = con1.CreateCommand();
            // string querystring1 = "Select * from Todos";
            // cmd1.CommandText = querystring1;
            // con1.Open();
            // OracleDataReader reader1 = cmd1.ExecuteReader();
            // reader1.Read();
            // return reader1[1].ToString();

            return "weeeaaakkk!!";
        }
    }
}