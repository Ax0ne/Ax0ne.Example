using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace Example.ConsoleApp.FileZip
{
    public static class FileZipTools
    {
        /// <summary>
        /// 解压Zip文件到指定的路径
        /// </summary>
        /// <param name="zipFilePath">压缩文件的路径</param>
        /// <param name="extractPath">解压的路径</param>
        public static void ExtractZipFile(string zipFilePath, string extractPath)
        {
            using (var zipFileToOpen = new FileStream(zipFilePath, FileMode.Open))
            using (var archive = new ZipArchive(zipFileToOpen, ZipArchiveMode.Read))
            {
                if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
                archive.ExtractToDirectory(extractPath);
            }
        }
    }

}
