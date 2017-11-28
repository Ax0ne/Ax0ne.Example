using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Example.Payment.Alipay;
using RSA = Example.Payment.Alipay.RSA;

namespace Example.Payment
{
    internal class AliPayUtils
    {
        //支付宝网关地址（新）
        private static readonly string GatewayNew = "https://mapi.alipay.com/gateway.do?";
        //商户的私钥
        private static readonly string PrivateKey = AliPayConfig.PrivateKey;
        //编码格式
        private static readonly string InputCharset = AliPayConfig.InputCharset.Trim().ToLower();
        //签名方式
        private static readonly string SignType = AliPayConfig.SignType.Trim().ToUpper();
        /// <summary>
        /// 生成请求时的签名
        /// </summary>
        /// <param name="sPara">请求给支付宝的参数数组</param>
        /// <returns>签名结果</returns>
        private static string BuildRequestMysign(Dictionary<string, string> sPara)
        {
            //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            var prestr = CreateLinkString(sPara);

            //把最终的字符串签名，获得签名结果
            string mysign;
            switch (SignType)
            {
                case "RSA":
                    mysign = RSA.Sign(prestr, PrivateKey, InputCharset);
                    break;
                default:
                    mysign = "";
                    break;
            }
            return mysign;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
        {
            //签名结果
            string mysign;

            //过滤签名参数数组
            var sPara = FilterPara(sParaTemp);

            //获得签名结果
            mysign = BuildRequestMysign(sPara);

            //签名结果与签名方式加入请求提交参数组中
            sPara.Add("sign", mysign);
            sPara.Add("sign_type", SignType);

            return sPara;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        // ReSharper disable once UnusedMember.Local
        private static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            //待签名请求参数数组
            var sPara = BuildRequestPara(sParaTemp);

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = CreateLinkStringUrlencode(sPara, code);
            return strRequestData;
        }

        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
        {
            //待请求参数数组
            var dicPara = BuildRequestPara(sParaTemp);

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + GatewayNew + "_input_charset=" + InputCharset + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            return sbHtml.ToString();
        }

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <returns>时间戳字符串</returns>
        public static string Query_timestamp()
        {
            var url = GatewayNew + "service=query_timestamp&partner=" + AliPayConfig.Partner + "&_input_charset=" + AliPayConfig.InputCharset;

            var reader = new XmlTextReader(url);
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);

            var selectSingleNode = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key");
            var encryptKey = selectSingleNode?.InnerText;
            return encryptKey;
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            return dicArrayPre.Where(temp => temp.Key.ToLower() != "sign"
                                             && temp.Key.ToLower() != "sign_type"
                                             && !string.IsNullOrEmpty(temp.Value))
                .ToDictionary(temp => temp.Key, temp => temp.Value);
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="dicArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            var prestr = new StringBuilder();
            //dicArray.Select(m => $"{m.Key}={m.Value}").Aggregate((seed, item) => $"{seed}&{item}");
            foreach (var temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            var nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);
            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="dicArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            var prestr = new StringBuilder();
            foreach (var temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 写日志，方便测试（看网站需求，也可以改成把记录存入数据库）
        /// </summary>
        /// <param name="sWord">要写入日志里的文本内容</param>
        public static void LogResult(string sWord)
        {
            var strPath = AliPayConfig.LogPath + "\\" + "alipay_log_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            var fs = new StreamWriter(strPath, false, Encoding.Default);
            fs.Write(sWord);
            fs.Close();
        }

        /// <summary>
        /// 获取文件的md5摘要
        /// </summary>
        /// <param name="sFile">文件流</param>
        /// <returns>MD5摘要结果</returns>
        public static string GetAbstractToMD5(Stream sFile)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var result = md5.ComputeHash(sFile);
            var sb = new StringBuilder(32);
            foreach (var t in result)
            {
                sb.Append(t.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取文件的md5摘要
        /// </summary>
        /// <param name="dataFile">文件流</param>
        /// <returns>MD5摘要结果</returns>
        public static string GetAbstractToMD5(byte[] dataFile)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var result = md5.ComputeHash(dataFile);
            var sb = new StringBuilder(32);
            foreach (byte t in result)
            {
                sb.Append(t.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        public class AliPayConfig
        {
            /// <summary>
            ///  合作身份者ID，签约账号
            /// </summary>
            public static string Partner = ConfigurationManager.AppSettings.Get("Alipay_Partner");

            /// <summary>
            /// 收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
            /// </summary>
            public static string SellerId = Partner;

            /// <summary>
            /// 商户的私钥文件路径,原始格式，RSA公私钥生成：https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.nBDxfy&treeId=58&articleId=103242&docType=1
            /// </summary>
            public static string PrivateKey = HttpRuntime.AppDomainAppPath + "App_Data\\rsa_private_key.pem";

            /// <summary>
            ///支付宝的公钥文件路径，查看地址：https://b.alipay.com/order/pidAndKey.htm 
            /// </summary>
            public static string AlipayPublicKey = HttpRuntime.AppDomainAppPath + "App_Data\\alipay_public_key.pem";

            /// <summary>
            /// 服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
            /// </summary>
            public static string NotifyUrl = ConfigurationManager.AppSettings.Get("Alipay_NotifyUrl");//"http://vb14965593.51mypc.cn/payment/callback";

            /// <summary>
            ///页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问
            /// </summary>
            public static string ReturnUrl = ConfigurationManager.AppSettings.Get("Alipay_ReturnUrl"); //"http://vb14965593.51mypc.cn/Payment/PaySuccess";

            /// <summary>
            /// 签名方式
            /// </summary>
            public static string SignType = "RSA";

            /// <summary>
            /// 调试用，创建TXT日志文件夹路径，见Alipaycs类中的LogResult(string sWord)打印方法。
            /// </summary>
            public static string LogPath = HttpRuntime.AppDomainAppPath + "logs\\";

            /// <summary>
            /// 字符编码格式 目前支持 gbk 或 utf-8
            /// </summary>
            public static string InputCharset = "utf-8";

            /// <summary>
            /// 支付类型
            /// </summary>
            public static string PaymentType = "1";

            /// <summary>
            /// 调用的接口名
            /// </summary>
            public static string WebService = "create_direct_pay_by_user";

            public static string WapService = "alipay.wap.create.direct.pay.by.user";

            //↓↓↓↓↓↓↓↓↓↓请在这里配置防钓鱼信息，如果没开通防钓鱼功能，请忽视不要填写 ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            //防钓鱼时间戳  若要使用请调用类文件submit中的Query_timestamp函数
            public static string AntiPhishingKey = "";

            //客户端的IP地址 非局域网的外网IP地址，如：221.0.0.1
            public static string ExterInvokeIp = "";

            //↑↑↑↑↑↑↑↑↑↑请在这里配置防钓鱼信息，如果没开通防钓鱼功能，请忽视不要填写 ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        }
    }
}
