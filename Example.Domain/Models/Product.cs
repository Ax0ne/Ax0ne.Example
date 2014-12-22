/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/7/17 16:35:21
 *  FileName:Product.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Example.Domain.Models
{
    public class Product
    {
        [ScaffoldColumn(false)] //在生成编辑表单时，跳过这个Id属性
        public int Id { get; set; }
        [Required,DefaultValue("Ax0ne")]
        public string Name { get; set; }
        [DefaultValue(1.0)]
        public decimal Price { get; set; }
        [DefaultValue(ProductCategory.Foodstuff)]
        public ProductCategory Category { get; set; }
        [DefaultValue(1.0)]
        public decimal ActualCost { get; set; }
    }
    [Flags]
    public enum ProductCategory
    {
        [Description("服装")]
        Clothing,
        [Description("电子")]
        Electronic,
        [Description("食品")]
        Foodstuff
    }
}
