/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/12/24 18:18:42
 *  FileName:Utils.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace Example.Infrastructure
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public class Utils
    {
        #region Invoke Api
        /// <summary>
        /// Api调用 [GET] , 异常或其他错误返回default(T)
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="absoluteUri">请求接口地址 例:Controller/Action?p1=abc</param>
        /// <returns></returns>
        public static T InvokeApi<T>(string absoluteUri)
        {
            return InvokeApi<T>(absoluteUri, null, HttpMethod.Get);
        }
        /// <summary>
        /// Api调用 [POST] , 异常或其他错误返回default(T)
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="absoluteUri">请求接口地址 例:Controller/Action</param>
        /// <param name="value">POST FormBody参数</param>
        /// <returns></returns>
        public static T InvokeApi<T>(string absoluteUri, object value)
        {
            return InvokeApi<T>(absoluteUri, value, HttpMethod.Post);
        }
        // WebApi 调用 内部封装
        private static T InvokeApi<T>(string absoluteUri, object value, HttpMethod method)
        {
            var responseResult = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("");
                if (method == HttpMethod.Post)
                {
                    responseResult = httpClient.PostAsJsonAsync(absoluteUri, value).Result;
                }
                else if (method == HttpMethod.Get)
                {
                    responseResult = httpClient.GetAsync(absoluteUri).Result;
                }
            }
            try
            {
                if (!responseResult.IsSuccessStatusCode) return default(T);
                if (typeof(T).Name == "String")
                    return (T)Convert.ChangeType(responseResult.Content.ReadAsStringAsync().Result, typeof(T));
                return responseResult.Content.ReadAsAsync<T>().Result;
            }
            catch
            {
                return default(T);
            }
        } 
        #endregion
    }
}
