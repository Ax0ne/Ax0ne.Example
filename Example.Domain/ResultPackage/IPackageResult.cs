/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/12/23 11:01:16
 *  FileName:IPackageResult.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain
{
    public interface IPackageResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        bool IsSuccess { get; set; }
        int ResultCode { get; set; }
        string Message { get; set; }
    }
}
