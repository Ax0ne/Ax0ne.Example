using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Example.Payment.Interface;
using Example.Payment.Models;
using static System.String;
/*
    <!-- 微信支付配置 -->
    <add key="WXPAY_WEB_APPID" value="wx229bcd8b94ef851b"/>
    <add key="WXPAY_WEB_MCH_ID" value="1384526302"/>
    <add key="WXPAY_WEB_NOTIFY_URL" value="http://xmall.xianshuabao.com/payment/wxpaycallback"/>
    <add key="WXPAY_WEB_KEY" value="0D68B110365411F427125D45E4D77699"/>
    <add key="WXPAY_PAY_URL" value="https://api.mch.weixin.qq.com/pay/unifiedorder"/>
    <add key="WXPAY_ORDERQUERY_URL" value="https://api.mch.weixin.qq.com/pay/orderquery"/>
 */

namespace Example.Payment
{
    /// <summary>
    /// 微信支付核心代码实现
    /// </summary>
    public class WxPay : IWxPay
    {
        /// <summary>
        /// 生成微信支付 最后返回二维码链接地址
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        /// <param name="productName">产品名称</param>
        /// <param name="totalPrice">总价</param>
        /// <param name="customerIp">支付ip地址</param>
        /// <param name="payType">微信支付的类型 目前是二维码支付</param>
        /// <returns></returns>
        public string BuildWxPay(string orderNumber, string productName, decimal totalPrice, string customerIp, WxPayType payType = WxPayType.Native)
        {
            return WxPayUtils.UnifiedOrder(orderNumber, productName, totalPrice, customerIp, payType);
        }
        /// <summary>
        /// 支付完成的异步通知处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="payResult"></param>
        /// <returns></returns>
        public bool VerifyNotify(HttpRequestBase request, out WxPayResult payResult)
        {
            var verifyResult = false;
            payResult = new WxPayResult();

            var requestXml = WxPayUtils.GetRequestXmlData(request);
            var dic = WxPayUtils.FromXml(requestXml);

            var returnCode = GetValueFromDic<string>(dic, "return_code");

            if (!IsNullOrEmpty(returnCode) && returnCode == "SUCCESS") // 通讯成功
            {
                var result = WxPayUtils.WxPayNotifyValidation(dic);
                if (result)
                {
                    var transactionid = GetValueFromDic<string>(dic, "transaction_id");

                    if (!IsNullOrEmpty(transactionid))
                    {
                        var queryXml = WxPayUtils.BuildQueryRequest(transactionid, dic);
                        var queryResult = HttpUtils.Post(WxPayUtils.WxPayConfig.WXPAY_ORDERQUERY_URL, queryXml);
                        var queryReturnDic = WxPayUtils.FromXml(queryResult);

                        if (ValidatonQueryResult(queryReturnDic)) // 查询成功
                        {
                            verifyResult = true;
                            payResult.OrderNumber = GetValueFromDic<string>(dic, "out_trade_no");
                            payResult.TotalPrice = GetValueFromDic<decimal>(dic, "total_fee") / 100;
                            payResult.TradeNo = transactionid;
                            payResult.TradeStatus = GetValueFromDic<string>(dic, "result_code");
                            payResult.ReturnXml = BuildReturnXml("OK", "成功");
                        }
                        else
                            payResult.ReturnXml = BuildReturnXml("FAIL", "订单查询失败");
                    }
                    else
                        payResult.ReturnXml = BuildReturnXml("FAIL", "支付结果中微信订单号不存在");
                }
                else
                    payResult.ReturnXml = BuildReturnXml("FAIL", "签名失败");
            }
            else
            {
                dic.TryGetValue("return_msg", out string returnmsg);
                throw new WxPayException("异步通知错误：" + returnmsg);
            }
            return verifyResult;
        }

        #region private methods

        private static string BuildReturnXml(string code, string returnMsg)
        {
            return $"<xml><return_code><![CDATA[{code}]]></return_code><return_msg><![CDATA[{returnMsg}]]></return_msg></xml>";
        }
        private static bool ValidatonQueryResult(SortedDictionary<string, string> dic)
        {
            var result = false;

            if (dic.ContainsKey("return_code") && dic.ContainsKey("return_code"))
            {
                if (dic["return_code"] == "SUCCESS" && dic["result_code"] == "SUCCESS")
                    result = true;
            }
            if (!result)
            {
                var sb = new StringBuilder();
                foreach (var item in dic.Keys)
                {
                    sb.Append(item + ":" + dic[item] + "|");
                }
            }
            return result;
        }
        private T GetValueFromDic<T>(SortedDictionary<string, string> dic, string key)
        {
            string val;
            dic.TryGetValue(key, out val);

            var returnVal = default(T);
            if (val != null)
                returnVal = (T)Convert.ChangeType(val, typeof(T));

            return returnVal;
        }
        #endregion
    }

}
