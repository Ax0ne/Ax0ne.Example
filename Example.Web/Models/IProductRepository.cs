/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/7/17 16:09:38
 *  FileName:IProductRepository.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Web.Models
{
    public interface IProductRepository
    {
        /// <summary>
        /// 获得所有所有产品
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetAll();
        /// <summary>
        /// 根据产品Id获取单个产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product Get(int id);
        /// <summary>
        /// 新增一个产品
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Product Add(Product item);
        /// <summary>
        /// 根据产品Id移除一个产品
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);
        /// <summary>
        /// 更新一个产品
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Update(Product item);
    } 
}
