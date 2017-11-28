/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/12/23 11:04:01
 *  FileName:PackageResult.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain
{
    public class PackageResult<T> : List<T>, IPackageResult
    {
        public PackageResult(T item)
        {
            Add(item);
        }

        public PackageResult(IEnumerable<T> collection)
        {
            AddRange(collection);
        }
        public bool IsSuccess { get; set; }

        public int ResultCode { get; set; }

        public string Message { get; set; }
    }
}
