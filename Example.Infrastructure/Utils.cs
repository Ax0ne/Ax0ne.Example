/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/12/24 18:18:42
 *  FileName:Utils.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Text;
using System.IO;
using System.Net.Http;

namespace Example.Infrastructure
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public static class Utils
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

        #region File Operator
        /// <summary>
        /// 把内容写入文件并清除原有内容
        /// </summary>
        /// <param name="path">文件完整路径名</param>
        /// <param name="content">内容</param>
        /// <param name="encoding">字符编码</param>
        public static void WriteFile(string path, string content, Encoding encoding)
        {
            WriteFile(path, content, encoding, false);
        }
        /// <summary>
        /// 把内容写入文件的最后
        /// </summary>
        /// <param name="path">文件完整路径名</param>
        /// <param name="content">内容</param>
        /// <param name="encoding">字符编码</param>
        public static void WriteFileAppend(string path, string content, Encoding encoding)
        {
            WriteFile(path, content, encoding, true);
        }
        private static void WriteFile(string path, string content, Encoding encoding, bool isAppend)
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var writer = new StreamWriter(path, isAppend, encoding ?? Encoding.UTF8))
            {
                writer.Write(content);
            }
        }
        public static void CopyFile(string source, string destination)
        {
            var directory = Path.GetDirectoryName(destination);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.Copy(source, destination);
        } 
        #endregion


    }
}
