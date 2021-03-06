﻿using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using Example.ConsoleApp.FileZip;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;

namespace Example.ConsoleApp
{
    using System.IO;
    using System.Net;

    internal class Package<T>
    {
        public T MyType { get; set; }
        /// <summary>
        /// Counter
        /// <code>public int Count { get; set; }</code>
        /// </summary>
        public int Count { get; set; }
    }
    internal class Invoke<T>
    {
        public void Convert(object value)
        {
            var valueType = (T)value;
            var myType = typeof(T);
            var propertyInfo = myType.GetProperty("Count");
            var qlx = value as Package<object>;
            var propertyValue = myType.GetProperty("Count").GetValue(123, null);
        }
    }

    struct MyStruct
    {
        public MyStruct(int age) : this()
        {
            Age = age;
        }

        public int Age { get; set; }

    }
    internal class Program
    {
        //static ILog log = LogManager.GetLogger("Ax0neTestLog4net");
        
        [Conditional("CONTRACTS_FULL")]
        private static void Main(string[] args)
        {
            var asd = AppDomain.CurrentDomain.BaseDirectory;
            asd += ".png";
            var fileSufixx = asd.Substring(asd.LastIndexOf('.') + 1);

            //WebClient client = new WebClient();
            ////client.BaseAddress = "http://www.2345.com/help/pic/step/ff.png";
            //client.DownloadFile("http://www.2345.com/help/pic/step/ff.png", @"C:\Users\Ax0ne\Desktop\ProcessMonitor");
            var myStruct = new MyStruct { Age = 11 };
            const string fileName = "as.d.as.da.sd.png";
            Console.WriteLine(DateTime.Now.AddMonths(12));
            Console.WriteLine(fileName.Substring(fileName.LastIndexOf('.') + 1));
            var threadPoolExample = new ThreadPoolExample();
            threadPoolExample.Launch();
            Invoke<Package<int>> invoke = new Invoke<Package<int>>();
            var obj = new Package<int> { Count = 123 };
            invoke.Convert(obj);
            //RunDoc();
            Console.ReadLine();
            return;
            //var ss =@<img alt="CNIGCLogo.png" src="http://198.18.0.238:8094/uploadfiles/SumCMSImage/UserImage/20141024/201410241603590.png" title="CNIGCLogo.png"/>;
            string regImg = "<img[^>]+src=['\"]?([^'\"]*).*/>";
            string ss = Console.ReadLine();
            Match r = Regex.Match(ss, regImg);

            if (r.Success && r.Groups.Count > 1)
            {
                Console.WriteLine(r.Groups[1].Value);
            }

            Console.ReadLine();
        }

        private static void RunDoc()
        {
            string dirpath = @"F:\Ax0ne\Example\Ax0ne.Example\Example.ConsoleApp";
            string outputpath = @"output.txt";

            DocTreeHelper.PrintTree(dirpath);
            DocTreeHelper.PrintTree(dirpath, outputpath);

            Console.WriteLine("Output Finished");
            Console.WriteLine("输出完毕");
            Console.ReadLine();
        }
        /// <summary>
        ///     邀请码生成 规则:
        ///     <para>邀请码由三部分组成：来源标识(1位)+代数标识(3位)+唯一码标识(6位)</para>
        /// </summary>
        /// <param name="sourceType">邀请码来源</param>
        /// <param name="count">生成多少个邀请码</param>
        /// <param name="eraNumber">邀请码代数 1表示1代,2表示2代,以此类推</param>
        /// <returns></returns>
        public static string[] GenerateInviteCode(int sourceType, int count = 1, int eraNumber = 1)
        {
            // 邀请码生成函数 AddTime:20141024 15:10 By:Ax0ne
            // 容错判断
            if (eraNumber > 999)
                eraNumber = 999;
            if (count < 1) count = 1;
            if (eraNumber < 1) eraNumber = 1;
            // 前缀 = 来源标识(1位)+代数标识(3位)
            string prefix = sourceType + (1000 + eraNumber).ToString().Substring(1);
            char[] chars =
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
                'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            var result = new string[count];
            for (int c = 0; c < count; c++)
            {
                // 得到GUID字符串
                string guidString = Guid.NewGuid().ToString();
                // 得到GUID哈希码
                int hashCode = guidString.GetHashCode();
                // 拆分GUID成字符串数组
                string[] guidArray = guidString.Split('-');
                // 得到GUID最后一段唯一性最高的字符串
                string lastPartialGuid = guidArray[guidArray.Length - 1];
                string randomChar = string.Empty;
                for (int i = 0; i < guidArray.Length; i++)
                {
                    string tempString = guidArray[i];
                    // 得到GUID字符串每段最后一个字符
                    randomChar += tempString.Substring(tempString.Length - 1);
                }
                // 根据GUID哈希码做随机数生成的种子
                int randomNumber = new Random(hashCode).Next(chars.Length - 1);
                randomChar += chars[randomNumber];
                // 得到大写格式的字符串
                result[c] = (prefix + randomChar).ToUpperInvariant();
            }
            // 返回生成的结果
            return result;
        }
    }
}