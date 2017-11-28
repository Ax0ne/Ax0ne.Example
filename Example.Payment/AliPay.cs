using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Example.Payment.Interface;
using Example.Payment.Models;
using Example.Payment.Alipay;

namespace Example.Payment
{
    public class AliPay : IAliPay
    {
        public string BuildAliPay(string orderNumber, string productName, decimal totalPrice, AliPayType type = AliPayType.Web)
        {
            var sortParams = new SortedDictionary<string, string>
            {
                {"service", type == AliPayType.Web?AliPayUtils.AliPayConfig.WebService:AliPayUtils.AliPayConfig.WapService},
                {"partner", AliPayUtils.AliPayConfig.Partner},
                {"seller_id", AliPayUtils.AliPayConfig.SellerId},
                {"_input_charset", AliPayUtils.AliPayConfig.InputCharset.ToLower()},
                {"payment_type", AliPayUtils.AliPayConfig.PaymentType},
                {"notify_url", AliPayUtils.AliPayConfig.NotifyUrl},
                {"return_url", AliPayUtils.AliPayConfig.ReturnUrl},
                {"anti_phishing_key", AliPayUtils.AliPayConfig.AntiPhishingKey},
                {"exter_invoke_ip", AliPayUtils.AliPayConfig.ExterInvokeIp},
                {"out_trade_no", orderNumber},
                {"subject", productName},
                {"total_fee", totalPrice.ToString("F2")},
                {"body", null}
            };
            return AliPayUtils.BuildRequest(sortParams, "get", "确认");
        }

        public bool VerifyNotify(HttpRequestBase request, out AliPayResult payResult)
        {
            if (request.Form == null)
                throw new AliPayException("request.Form is null");
            var parameters = new SortedDictionary<string, string>();
            foreach (var key in request.Form.AllKeys)
            {
                parameters.Add(key, request.Form[key]);
            }
            var notifyId = parameters["notify_id"];
            var sign = parameters["sign"];
            var isValid = new Notify().Verify(parameters, notifyId, sign);
            if (!isValid)
                throw new AliPayException($"回调签名验证不通过:{sign}");
            var status = parameters["trade_status"];
            payResult = new AliPayResult();
            if (status == "TRADE_SUCCESS" || status == "TRADE_FINISHED")
            {
                payResult.OrderNumber = parameters["out_trade_no"];
                payResult.TradeNo = parameters["trade_no"];
                payResult.TotalPrice = decimal.Parse(parameters["total_fee"]);
                return true;
            }
            return false;
        }
    }
}
