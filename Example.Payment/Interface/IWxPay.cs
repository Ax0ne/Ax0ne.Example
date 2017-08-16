using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Example.Payment.Models;

namespace Example.Payment.Interface
{
    public interface IWxPay
    {

        /// <summary>
        /// 创建微信支付
        /// </summary>
        /// <param name="orderNumber">订单号</param>
        /// <param name="productName">产品名称</param>
        /// <param name="totalPrice">总金额，单位分</param>
        /// <param name="customerIp">调用IP</param>
        /// <param name="payType">交易类型（公众号支付、扫码支付、APP、WAP支付）默认扫码支付</param>
        /// <returns>
        /// 扫码支付：返回支付URL
        /// APP支付：返回Json字符串，包含支付sdk支付参数
        /// 公众号支付&Wap支付：暂未实现
        /// </returns>
        string BuildWxPay(string orderNumber, string productName, decimal totalPrice, string customerIp, WxPayType payType = WxPayType.Native);

        /// <summary>
        /// 微信支付异步通知验证
        /// </summary>
        /// <param name="request">HTTP请求</param>
        /// <param name="payResult">当验证成功后，获取主要返回参数</param>
        /// <returns>验证结果</returns>
        bool VerifyNotify(HttpRequestBase request, out WxPayResult payResult);
    }
}
