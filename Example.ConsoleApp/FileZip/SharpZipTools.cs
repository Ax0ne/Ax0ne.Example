/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2015/1/7 11:37:46
 *  FileName:SharpZipTools.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/

using System.IO;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Example.ConsoleApp.FileZip
{
    /// <summary>
    /// SharpZip压缩解压缩工具类
    /// </summary>
    public static class SharpZipTools
    {
        /// <summary>
        /// 解压Zip文件到指定的路径
        /// </summary>
        /// <param name="zipFilePath">压缩文件的路径</param>
        /// <param name="extractPath">解压的路径</param>
        public static void ExtractZipFile(string zipFilePath, string extractPath)
        {
            ZipFile zf = null;
            try
            {
                var fs = File.OpenRead(zipFilePath);
                zf = new ZipFile(fs);
                //if (!String.IsNullOrEmpty(password))
                //{
                //    zf.Password = password; // AES encrypted entries are handled automatically
                //}
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue; // Ignore directories
                    }
                    var entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    var buffer = new byte[4096]; // 4K is optimum
                    var zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    var fullZipToPath = Path.Combine(extractPath, entryFileName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (!string.IsNullOrEmpty(directoryName) && directoryName.Length > 0) Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }
    }

}
