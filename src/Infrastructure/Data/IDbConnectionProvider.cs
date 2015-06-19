using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Infrastructure.Data
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetNewConnection();
    }

    public class DbConnectionProvider : IDbConnectionProvider
    {
        private string _connectionString;

        public DbConnectionProvider()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Boon.Plan.ConnectionString"].ConnectionString;
        }

        public IDbConnection GetNewConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
