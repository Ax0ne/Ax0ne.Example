/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/6/7 9:22:08
 *  FileName:TestCode.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.ConsoleApp
{
    class TestCode
    {
        public IEnumerable<int> TestYield()
        {
            for (int i = 0; i < 100; i++)
            {
                yield return i;
            }
        }
    }
}
