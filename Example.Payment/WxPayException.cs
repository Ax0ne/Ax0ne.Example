using System;

namespace Example.Payment
{
    public class WxPayException : Exception
    {
        public WxPayException(string message) : base("微信支付异常：" + message)
        {
        }
    }
}
