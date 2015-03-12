using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Example.Infrastructure
{

    public class ImageHelper
    {
        /// <summary>
        /// 下载远程服务器上的图片
        /// </summary>
        /// <param name="uri">图片资源地址</param>
        /// <param name="fileName">存放本地的完整路径和文件名</param>
        public static void DownloadRemoteImageFile(string uri, string fileName)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            var response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                // if the remote file was found, download oit
                using (var inputStream = response.GetResponseStream())
                using (Stream outputStream = File.OpenWrite(fileName))
                {
                    var buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }
        }

        public static string RemoteUploadFileNew(string remoteUrl,string filePath, string fileName = "")
        {
            var boundary = "----------" + DateTime.Now.Ticks.ToString("x");

            // Build up the post message header  
            var sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append(fileName);
            sb.Append("\"; filename=\"");
            sb.Append(Path.GetFileName(filePath));
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");

            var postHeader = sb.ToString();
            var postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
            var webrequest = (HttpWebRequest)WebRequest.Create(remoteUrl);

            webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webrequest.Method = "POST";
            webrequest.AllowWriteStreamBuffering = false;
            // Build the trailing boundary string as a byte array  
            // ensuring the boundary appears on a line by itself  
            var boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var length = postHeaderBytes.Length + fileStream.Length + boundaryBytes.Length;
            webrequest.ContentLength = length;

            var requestStream = webrequest.GetRequestStream();

            // Write out our post header  
            requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

            // Write out the file contents  
            var buffer = new byte[fileStream.Length];
            var bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                requestStream.Write(buffer, 0, bytesRead);

            // Write out the trailing boundary  
            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            try
            {
                using (var response = webrequest.GetResponse().GetResponseStream())
                {
                    if (response != null)
                    {
                        using (var reader = new StreamReader(response))
                        {
                            var result = reader.ReadToEnd();
                            return result;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    throw new WebException(remoteUrl + "|1|" + response.StatusDescription, ex);
                }
                throw new Exception(remoteUrl + "|2|" + ex.Message);
            }
            catch (Exception exception)
            {
                throw new Exception(remoteUrl + "|3|" + exception.Message);
            }
            finally
            {
                requestStream.Dispose();
                fileStream.Dispose();
                System.IO.File.Delete(filePath); // 删除本地图片
            }
        }

        /// <summary>
        /// 图片压缩处理
        /// </summary>
        /// <param name="path"></param>
        /// <param name="imageCompressRatio"></param>
        public static string CompressImage(string path, int imageCompressRatio)
        {
            //Image image = Image.FromStream(imageStream);
            var image = Image.FromFile(path);
            var imageFormat = image.RawFormat;

            int width = 0, height = 0;

            width = image.Width; //SumCMS.Common.SystemConfig.ImageCompressionWidth; // 从配置文件获取宽度
            height = image.Height;
            //double whRatio = 1d; // width and height ratio default 1:1
            //if (image.Width > width)
            //{
            //    whRatio = (double)image.Width / (double)image.Height; // 保留小数
            //    double heightA = (image.Width - image.Height) / whRatio;
            //    height = (int)heightA;
            //    // 补差算法, 比如 w:1000 h:1000
            //    while ((double)width / (double)height > whRatio - 0.2)
            //    {
            //        height += 10;
            //    }
            //}
            //else
            //{
            //    width = image.Width;
            //}
            var bitmap = new Bitmap(image, new Size(width, height));
            //Graphics g = Graphics.FromImage(bitmap);
            //g.CompositingQuality = CompositingQuality.HighQuality;
            //g.SmoothingMode = SmoothingMode.HighQuality;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.DrawImage(image, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            //g.Dispose(); // dispose resource
            var eps = new EncoderParameters();
            var qy = new long[1];
            qy[0] = imageCompressRatio; // 压缩的比例 1-100
            //if (length <= 1024 * 100) qy[0] = 90; // 如果图片大小<=100K 就按90%的比例压缩
            var ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            eps.Param[0] = ep;
            var newFilePath = Regex.Replace(path, "_", "");
            try
            {
                var arrayIci = ImageCodecInfo.GetImageEncoders();
                var jpegIcIinfo = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));
                if (jpegIcIinfo != null)
                {
                    //bitmap.Save(stream, jpegICIinfo, eps);
                    bitmap.Save(newFilePath, jpegIcIinfo, eps);
                }
                else
                {
                    bitmap.Save(newFilePath, imageFormat);
                }
            }
            finally
            {
                image.Dispose();
                bitmap.Dispose();
                System.IO.File.Delete(path);
            }
            return newFilePath;
        }
    }
}
