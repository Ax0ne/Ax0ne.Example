/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 16:50:20
 *  FileName:DbContextBase.cs
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
    public class DbContextBase:DbContext
    {
        public DbContextBase(string nameOrConnectionstring)
            : base(nameOrConnectionstring)
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<DisplayAttribute, string>("ColumnAnnotation",
                    (entityType, attribute) =>
                    {
                        var attr = attribute.Single();
                        if (!string.IsNullOrWhiteSpace(attr.Name)) return attr.Name;
                        else if (!string.IsNullOrWhiteSpace(attr.Description)) return attr.Description;
                        return "None";
                    }),
                new AttributeToTableAnnotationConvention<DisplayAttribute, string>("TableAnnotation",
                    (entityType, attribute) =>
                    {
                        var attr = attribute.Single();
                        if (!string.IsNullOrWhiteSpace(attr.Name)) return attr.Name;
                        else if (!string.IsNullOrWhiteSpace(attr.Description)) return attr.Description;
                        return "None";
                    })
                );
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
