/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/17 15:54:21
 *  FileName:Entity.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Example.Domain
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class Entity:IEntity<int>
    {
        [Key]
        public int Id { get; set; }
    }
}
