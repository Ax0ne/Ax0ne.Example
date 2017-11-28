/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/7/17 17:00:01
 *  FileName:ExampleContextInitializer.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Example.Domain.Models;

namespace Example.Domain
{
    public class ExampleContextInitializer:DropCreateDatabaseIfModelChanges<ExampleContext>
    {
        protected override void Seed(ExampleContext context)
        {
            var products = new List<Product>()             
            { 
                new Product() { Name = "Tomato Soup", Price = 1.39M, ActualCost = .99M }, 
                new Product() { Name = "Hammer", Price = 16.99M, ActualCost = 10 }, 
                new Product() { Name = "Yo yo", Price = 6.99M, ActualCost = 2.05M } 
            }; 

            products.ForEach(p => context.Products.Add(p)); 
            context.SaveChanges(); 

            var order = new Order() { Customer = "Bob" }; 
            var od = new List<OrderDetail>() 
            { 
                new OrderDetail() { Product = products[0], Quantity = 2, Order = order}, 
                new OrderDetail() { Product = products[1], Quantity = 4, Order = order } 
            }; 
            context.Orders.Add(order); 
            od.ForEach(o => context.OrderDetails.Add(o)); 

            context.SaveChanges(); 
        }
    }
}
