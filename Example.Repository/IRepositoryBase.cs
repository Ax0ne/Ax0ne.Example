/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 15:45:44
 *  FileName:IRepositoryBase.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Example.Domain;
using Example.Infrastructure;

namespace Example.Repository
{
    /// <summary>
    /// 仓储接口基类
    /// </summary>
    public interface IRepositoryBase<T>:IRepository<T> where T:Entity
    {
    }
}
