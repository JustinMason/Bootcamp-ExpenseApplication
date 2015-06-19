using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Bootcamp.Infrastructure;

namespace Bootcamp.Infrastructure.Data
{
    public interface IMetaUserRepository
    {
        IEnumerable<ApplicationUserRole> GetApplicationRolesByUserName(string accountName);
    }

    public class MetaUserRepository : IMetaUserRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public MetaUserRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<ApplicationUserRole> GetApplicationRolesByUserName(string accountName)
        {
            return new List<ApplicationUserRole>();
            using (var conn = _dbConnectionProvider.GetNewConnection())
            {
                conn.Open();

                var result = conn.GetList<ApplicationUserRole>(@"where usr_domainIdentity ='{0}'".FormatWith(accountName));
                return result;
            }

        }
    }
}
