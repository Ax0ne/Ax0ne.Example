/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/6/7 10:28:09
 *  FileName:SumClass.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Example.ConsoleApp
{
    class SumClass
    {
        public int Id { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionType { get; set; }

        public string Linkman { get; set; }
    }
}
