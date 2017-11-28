/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/7/28 11:04:12
 *  FileName:LogInfo.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Example.Domain.Models
{
    public class LogInfo
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [StringLength(255)]
        public string Thread { get; set; }
        [StringLength(50)]
        public string Level { get; set; }
        [StringLength(255)]
        public string Logger { get; set; }
        [StringLength(4000)]
        public string Message { get; set; }
        [StringLength(2000)]
        public string Exception { get; set; }

    }
}
