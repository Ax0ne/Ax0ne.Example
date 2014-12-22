/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/7/17 16:09:02
 *  FileName:Product.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    } 
}