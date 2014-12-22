/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/7/17 16:38:06
 *  FileName:Order.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/

namespace Example.Domain.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string Customer { get; set; }

        // Navigation property
        // 导航属性
        public ICollection<OrderDetail> OrderDetails { get; set; } 
    }
}
