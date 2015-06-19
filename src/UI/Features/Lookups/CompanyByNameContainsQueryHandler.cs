using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bootcamp.Infrastructure.Data;
using MediatR;
using Dapper;

namespace Bootcamp.UI.Features.Lookups
{
    public class CompanyByNameContainsQueryHandler : IRequestHandler<CompanyByNameContainsQuery, IEnumerable<CompanyListItemViewModel>>
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public CompanyByNameContainsQueryHandler(IDbConnectionProvider connectionProvider )
        {
            _connectionProvider = connectionProvider;
        }

        public IEnumerable<CompanyListItemViewModel> Handle(CompanyByNameContainsQuery message)
        {
            using (var conn = _connectionProvider.GetNewConnection())
            {
                conn.Open();
                var result = conn.Query<CompanyListItemViewModel>("select cmp_id, cmp_name from Company where cmp_name like @name", new {name = message.SearchText});
                return result;
            }
        }
    }
}