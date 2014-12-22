/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/7/17 16:10:08
 *  FileName:ProductRepository.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Example.Web.Models
{
    public class ProductRepository:IProductRepository
    {
        private List<Product> products = new List<Product>();
        private int _nextId = 1;
        public ProductRepository()
        {
            Add(new Product { Name = "Tomato soup", Category = "Groceries", Price = 1.39M });
            Add(new Product { Name = "Yo-yo", Category = "Toys", Price = 3.75M });
            Add(new Product { Name = "Hammer", Category = "Hardware", Price = 16.99M });
            Add(new Product { Name = "Computer", Category = "IT", Price = 4000M });
        }
        /// <inheritdoc />
        public IEnumerable<Product> GetAll()
        {
            return products;
        }
        /// <inheritdoc />
        public Product Get(int id)
        {
            return products.Find(p => p.Id == id);
        }
        /// <inheritdoc />
        public Product Add(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.Id = _nextId++;
            products.Add(item);
            return item;
        }
        /// <inheritdoc />
        public void Remove(int id)
        {
            products.RemoveAll(p => p.Id == id);
        }
        /// <inheritdoc />
        public bool Update(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = products.FindIndex(p => p.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            products.RemoveAt(index);
            products.Add(item);
            return true;
        } 
    }
}