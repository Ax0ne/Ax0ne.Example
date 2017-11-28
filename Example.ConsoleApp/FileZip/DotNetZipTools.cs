/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2015/1/7 11:37:59
 *  FileName:DotNetZipTools.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Ionic;
using Ionic.Zip;

namespace Example.ConsoleApp.FileZip
{
    /// <summary>
    /// DotNet压缩解压缩工具类
    /// </summary>
    public static class DotNetZipTools
    {
        /// <summary>
        /// 解压Zip文件到指定的路径
        /// </summary>
        /// <param name="zipFilePath">压缩文件的路径</param>
        /// <param name="extractPath">解压的路径</param>
        public static void ExtractZipFile(string zipFilePath, string extractPath)
        {
            var options = new ReadOptions { Encoding = Encoding.Default };
            using (var zipFile = ZipFile.Read(zipFilePath, options))
            {
                zipFile.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }
}
