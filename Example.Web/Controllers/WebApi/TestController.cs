using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Example.Web.Controllers.WebApi
{
    public class TestController : ApiController
    {
        public TestController()
        {
        }
        [System.Web.Http.HttpGet]
        public string GetData()
        {
            return "Get default method GetData";
        }

        [System.Web.Http.HttpPost]
        public string PostData()
        {
            return "Post default method PostData";
        }
        public string Ax0ne()
        {
            return "Ax0ne";
        }
        [System.Web.Http.HttpGet,System.Web.Http.ActionName("Ha")]
        public IHttpActionResult Method()
        {
            return Ok();
        }
        [System.Web.Http.ActionName("MethodA"),System.Web.Http.HttpGet]
        public string Method(int id)
        {
            return "string Method";
        }
    }
}
