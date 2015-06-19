using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediatR;

namespace Bootcamp.UI.Features.Lookups
{
    public class CompanyByNameContainsQuery : IRequest<IEnumerable<CompanyListItemViewModel>>
    {
        public string SearchText { get; set; }
    }
}