using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Example.Web.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult Index()
        {
            var ex = Server.GetLastError();

            if (Response.StatusCode == 404)
            {
                return RedirectToAction("Error404");
            }
            return View();
        }
        public ActionResult Error404()
        {
            return View();
        }
    }
}