/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 16:34:22
 *  FileName:UnitOfWork.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Infrastructure
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 事物-开始
        /// </summary>
        void BeginTransaction(System.Data.IsolationLevel level = System.Data.IsolationLevel.Chaos);
        /// <summary>
        /// 事物-提交
        /// </summary>
        /// <returns></returns>
        void Commit();
        /// <summary>
        /// 事物-回滚
        /// </summary>
        void Rollback();
        /// <summary>
        /// EF 跟踪状态级别事物,使用时操作实体对象的方法isSave参数要等于false
        /// </summary>
        /// <returns></returns>
        bool EFStateTransactionCommit();
    }
}
