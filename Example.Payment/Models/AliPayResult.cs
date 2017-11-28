using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Payment.Models
{
    public class AliPayResult
    {
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 交易订单号
        /// </summary>
        public string TradeNo { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
