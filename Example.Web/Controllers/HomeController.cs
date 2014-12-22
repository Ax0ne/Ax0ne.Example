using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Newtonsoft.Json;
using Example.Infrastructure;
using Example.Repository.Interface;

namespace Example.Web.Controllers
{
    public class HomeController : Controller
    {
        List<dynamic> list = new List<dynamic>(){
            new {Name="张三",Age=22,Gender="男"},
            new {Name="李四",Age=25,Gender="男"},
            new {Name="王五",Age=22,Gender="女"},
            new {Name="赵六",Age=22,Gender="女"}
        };
        public ActionResult Index()
        {
            //Microsoft.Practices.Unity.UnityContainer container = new Microsoft.Practices.Unity.UnityContainer();
            //var myContainer = container.Resolve<Infrastructure.UnityContainer>();
            //var userRepository = UnityContainer.Resolve<IUserRepository>();
            //System.Diagnostics.Debug.Assert(userRepository != null, userRepository.GetAddressByUserId(1));
            //ViewBag.Title = "Home Page";
            //int i = 0;
            //int m = 22 / i;
            return View();
        }
        public ActionResult Example()
        {
            var result = JsonConvert.SerializeObject(list);
            ViewBag.Persons = result;
            return View();
        }
        public ActionResult Admin()
        {

            string apiUri = Url.HttpRouteUrl("DefaultApi", new { controller = "admin", });
            ViewBag.ApiUrl = new Uri(Request.Url, apiUri).AbsoluteUri.ToString();
            return View();
        }
        
    }
}
