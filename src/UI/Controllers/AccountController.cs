using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bootcamp.Infrastructure.Security;

namespace Bootcamp.UI.Controllers
{


     [Authorize]
    public class AccountController : Controller
    {
         [ClaimsAuthorize(ClaimsAction.Read, "Account", "Claims")]
        public ActionResult Identity()
        {
            return View(HttpContext.User);
            
        }
    }
}