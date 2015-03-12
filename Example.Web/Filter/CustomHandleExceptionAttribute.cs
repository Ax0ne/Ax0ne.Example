using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Example.Infrastructure.Logging;

namespace Example.Web.Filter
{
    //[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class CustomHandleExceptionAttribute:HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Logger.Instance.Info(filterContext.Exception.Message,null);
            //filterContext.HttpContext.Trace.Write("Debug", "Trace.Write", filterContext.Exception);
        }
    }
}