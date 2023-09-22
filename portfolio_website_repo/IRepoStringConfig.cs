using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

// this class 
namespace portfolio_website_repo
{
    public interface IRepoStringConfig
    {
        string GetConnectionString(string connectionName);
    }
}