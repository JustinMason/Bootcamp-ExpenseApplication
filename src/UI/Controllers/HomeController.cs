using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bootcamp.UI.Features.Lookups;
using MediatR;

namespace Bootcamp.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test(string companyName)
        {
            var compQry = new CompanyByNameContainsQuery() {SearchText = companyName};
            IEnumerable<CompanyListItemViewModel> result = _mediator.Send(compQry);
            return new JsonResult()
            {
                Data = result
            };
        }
    }
}
