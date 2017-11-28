/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/7/17 17:24:47
 *  FileName:AuthorizeFilterAttribute.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Example.Web.Filter
{
    /// <summary>
    /// WebApi的权限过滤器
    /// </summary>
    public class AuthorizeFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }
    }

}