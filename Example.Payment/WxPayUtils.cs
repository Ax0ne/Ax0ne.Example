using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using Example.Payment.Models;

namespace Example.Payment
{
    public class WxPayUtils
    {
        /// <summary>
        /// 统一下单
        /// </summary>
        /// <returns></returns>
        public static string UnifiedOrder(string orderNumber, string productName, decimal totalPrice, string customerIp, WxPayType payType)
        {
            var totalPriceFen = Convert.ToInt32(totalPrice * 100);
            var requestXml = BuildRequest(orderNumber, productName, totalPriceFen, customerIp, payType);

            var resultXml = HttpUtils.Post(WxPayConfig.WXPAY_PAY_URL, requestXml);

            var dic = FromXml(resultXml);

            dic.TryGetValue("return_code", out string returnCode);
            dic.TryGetValue("return_msg", out string returnMsg);

            if (returnCode == "SUCCESS")
            {
                if (payType == WxPayType.Native)
                {
                    dic.TryGetValue("code_url", out string codeUrl);
                    if (!string.IsNullOrEmpty(codeUrl))
                        return codeUrl;
                    throw new WxPayException("未找到对应的二维码链接");
                }
                throw new WxPayException("JSAPI & WAP 未实现");
            }
            throw new WxPayException($"后台统一下单失败 Msg:{returnMsg} 订单号 {orderNumber}");
        }

        #region private methods

        private static string BuildRequest(string orderNumber, string productName, int totalPrice, string customerIp, WxPayType payType)
        {
            var dicParam = CreateParam(orderNumber, productName, totalPrice, customerIp, payType);

            var signString = CreateUrlParamString(dicParam);
            var key = WxPayConfig.WXPAY_WEB_KEY;
            var preString = signString + "&key=" + key;
            var sign = Md5(preString);
            dicParam.Add("sign", sign);
            return BuildForm(dicParam);
        }

        public static SortedDictionary<string, string> FromXml(string xml)
        {
            var sortDic = new SortedDictionary<string, string>();
            if (string.IsNullOrEmpty(xml))
            {
                throw new WxPayException("将空的xml串转换为WxPayData不合法!");
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            var nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                var xe = (XmlElement)xn;

                if (!sortDic.ContainsKey(xe.Name))
                    sortDic.Add(xe.Name, xe.InnerText);
            }
            return sortDic;
        }
        private static string BuildForm(SortedDictionary<string, string> dicParam)
        {
            var sbXml = new StringBuilder();
            sbXml.Append("<xml>");
            foreach (var temp in dicParam)
            {
                sbXml.Append("<" + temp.Key + ">" + temp.Value + "</" + temp.Key + ">");
            }

            sbXml.Append("</xml>");
            return sbXml.ToString();
        }
        private static string Md5(string str)
        {
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (var b in bs)
                sb.Append(b.ToString("x2"));
            return sb.ToString().ToUpper();
        }
        private static SortedDictionary<string, string> CreateParam(string orderNumber, string productName, int totalPrice, string customerIp, WxPayType payType)
        {
            var dic = new SortedDictionary<string, string>();
            dic.Add("appid", WxPayConfig.WXPAY_WEB_APPID); // 账号ID
            dic.Add("mch_id", WxPayConfig.WXPAY_WEB_MCH_ID); // 商户号
            dic.Add("nonce_str", Guid.NewGuid().ToString().Replace("-", "")); // 随机字符串
            dic.Add("body", productName); // 商品描述
            dic.Add("out_trade_no", orderNumber); // 商户订单号
            dic.Add("total_fee", totalPrice.ToString()); // 总金额
            dic.Add("spbill_create_ip", customerIp); // 终端IP
            dic.Add("notify_url", WxPayConfig.WXPAY_WEB_NOTIFY_URL); // 通知地址
            dic.Add("trade_type", payType.ToString().ToUpper()); // 交易类型
            return dic;
        }

        private static string CreateUrlParamString(SortedDictionary<string, string> dicArray)
        {
            var prestr = new StringBuilder();
            foreach (var temp in dicArray.OrderBy(o => o.Key))
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }
            var nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);
            return prestr.ToString();
        }
        #endregion

        public static bool WxPayNotifyValidation(SortedDictionary<string, string> dic)
        {
            var sign = GetValueFromDic<string>(dic, "sign");
            if (dic.ContainsKey("sign"))
            {
                dic.Remove("sign");
            }

            var tradeType = GetValueFromDic<string>(dic, "trade_type");
            var preString = CreateUrlParamString(dic);

            if (string.IsNullOrEmpty(tradeType))
            {
                var key = WxPayConfig.WXPAY_WEB_KEY;
                var preSignString = preString + "&key=" + key;
                var signString = Md5(preSignString).ToUpper();
                return signString == sign;
            }
            return false;
        }
        public static string GetRequestXmlData(HttpRequestBase request)
        {
            var stream = request.InputStream;
            var count = 0;
            var buffer = new byte[1024];
            var builder = new StringBuilder();
            while ((count = stream.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            stream.Flush();
            stream.Close();
            stream.Dispose();

            return builder.ToString();
        }

        public static string BuildQueryRequest(string transactionId, SortedDictionary<string, string> dic)
        {
            var dicParam = CreateQueryParam(transactionId);
            var signString = CreateUrlParamString(dicParam);

            var key = WxPayConfig.WXPAY_WEB_KEY;
            var preString = signString + "&key=" + key;

            var sign = Md5(preString);
            dicParam.Add("sign", sign);

            return BuildForm(dicParam);
        }

        #region private methods part 2

        private static SortedDictionary<string, string> CreateQueryParam(string transactionId)
        {
            var dic = new SortedDictionary<string, string>();
            dic.Add("appid", WxPayConfig.WXPAY_WEB_APPID);//公众账号ID
            dic.Add("mch_id", WxPayConfig.WXPAY_WEB_MCH_ID);//商户号
            dic.Add("nonce_str", Guid.NewGuid().ToString().Replace("-", ""));//随机字符串
            dic.Add("transaction_id", transactionId);//随机字符串
            return dic;
        }
        private static T GetValueFromDic<T>(SortedDictionary<string, string> dic, string key)
        {
            string val;
            dic.TryGetValue(key, out val);

            var returnVal = default(T);
            if (val != null)
                returnVal = (T)Convert.ChangeType(val, typeof(T));

            return returnVal;
        }

        #endregion
        /// <summary>
        /// 微信支付配置
        /// </summary>
        public class WxPayConfig
        {
            public static string WXPAY_PAY_URL = ConfigurationManager.AppSettings["WXPAY_PAY_URL"];//统一下单URL
            public static string WXPAY_ORDERQUERY_URL = ConfigurationManager.AppSettings["WXPAY_ORDERQUERY_URL"];

            public static string WXPAY_WEB_APPID = ConfigurationManager.AppSettings["WXPAY_WEB_APPID"];
            public static string WXPAY_WEB_MCH_ID = ConfigurationManager.AppSettings["WXPAY_WEB_MCH_ID"];
            public static string WXPAY_WEB_NOTIFY_URL = ConfigurationManager.AppSettings["WXPAY_WEB_NOTIFY_URL"];
            public static string WXPAY_WEB_KEY = ConfigurationManager.AppSettings["WXPAY_WEB_KEY"];
        }

    }
}
