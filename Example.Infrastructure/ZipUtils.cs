using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Example.Infrastructure
{
    public class ZipUtils
    {
        public void CreateZipFile(string filePath,string newFilePath)
        {
            var newZipStream = File.Create(newFilePath);
            try
            {
                var fs = File.OpenRead(filePath);
                var zipStream = new MemoryStream();
                var buffer = new byte[0x800];
                StreamUtils.Copy(fs, zipStream, buffer);
                fs.Dispose();
                //File.WriteAllText("");
                var fsEntry = File.OpenRead("");
                var msEntry = new MemoryStream();
                StreamUtils.Copy(fsEntry, msEntry, buffer);
                fsEntry.Close();
                File.Delete("");
                // 直接内存里操作 不必解压到磁盘
                var zf = new ZipFile(zipStream);
                zf.BeginUpdate();
                var userInfoDataSource = new UserInfoDataSource();
                userInfoDataSource.SetStream(msEntry);
                zf.Add(userInfoDataSource, "META-INF/ownerid");
                zf.CommitUpdate();
                zf.IsStreamOwner = false;
                using (var zipOutputStream = new ZipOutputStream(newZipStream))
                {
                    foreach (var file in zf)
                    {
                        var entry = (ZipEntry)file;
                        var zipEntry = new ZipEntry(entry.Name) { Size = entry.Size, DateTime = entry.DateTime };
                        zipOutputStream.PutNextEntry(zipEntry);
                        using (var entryStream = zf.GetInputStream(entry))
                        {
                            StreamUtils.Copy(entryStream, zipOutputStream, new byte[0x800]);
                        }
                    }
                    zf.Close();
                    zipOutputStream.Close();
                }
            }
            finally
            {
                newZipStream.Close();
            }
        }
        public class UserInfoDataSource : IStaticDataSource
        {
            private Stream _stream;
            public Stream GetSource()
            {
                return _stream;
            }
            public void SetStream(Stream inputStream)
            {
                _stream = inputStream;
                _stream.Position = 0;
            }
        }
    }
}
