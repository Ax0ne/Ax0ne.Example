using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Example.Payment.Models;

namespace Example.Payment.Interface
{
    public interface IAliPay
    {
        string BuildAliPay(string orderNumber, string productName, decimal totalPrice, AliPayType type = AliPayType.Web);
        bool VerifyNotify(HttpRequestBase request, out AliPayResult payResult);
    }
}
