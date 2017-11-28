/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 15:52:53
 *  FileName:IEntity.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Domain
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
