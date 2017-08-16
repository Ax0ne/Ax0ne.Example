using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Payment.Models
{
    public enum WxPayType
    {
        /// <summary>
        /// 公众号支付
        /// </summary>
        Jsapi = 0,
        /// <summary>
        /// 原生扫码支付
        /// </summary>
        Native,
        /// <summary>
        /// app支付
        /// </summary>
        App,
        /// <summary>
        /// wap支付（浏览器调用微信app，目前腾讯还未对外开放）
        /// </summary>
        Wap
    }
}
