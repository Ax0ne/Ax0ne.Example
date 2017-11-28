using System;

namespace Example.Payment
{
    public class AliPayException : Exception
    {
        public AliPayException(string message) : base("支付宝支付异常：" + message)
        {
        }
    }
}
