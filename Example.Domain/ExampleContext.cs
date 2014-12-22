/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/7/17 16:51:36
 *  FileName:ExampleContext.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure.Interception;
using Example.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Example.Domain
{
    public class ExampleContext:DbContextBase
    {
        static ExampleContext()
        {
#if DEBUG
            DbInterception.Add(new DbInterceptorCommand());
#endif
        }
        public ExampleContext()
            : base("Name=ExampleContext")
        {

        }
        #region DbSet Collection
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<LogInfo> Logs { get; set; } 
        #endregion

    }
}
