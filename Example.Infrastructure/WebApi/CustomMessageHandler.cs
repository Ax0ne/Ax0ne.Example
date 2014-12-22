/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/8/12 14:26:45
 *  FileName:CustomMessageHandler.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Example.Infrastructure.WebApi
{
    /// <summary>
    /// 自定义消息处理器
    /// </summary>
    public class CustomMessageHandler:DelegatingHandler
    {
        private static int count = 0;
        /// <summary>
        /// 以异步操作发送 HTTP 请求到内部管理器以发送到服务器。
        /// </summary>
        /// <param name="request">要发送到服务器的 HTTP 请求消息。</param>
        /// <param name="cancellationToken">取消操作的取消标记。</param>
        /// <returns>返回 <see cref="T:System.Threading.Tasks.Task`1" />。 表示异步操作的任务对象。</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            //HttpClientFactory.Create() // 使用方法
            count++;
            request.Headers.Add("X-Custom-Header", count.ToString());
            return base.SendAsync(request, cancellationToken);
        }
    }
}
