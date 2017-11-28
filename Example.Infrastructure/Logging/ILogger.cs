/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/3 10:44:02
 *  FileName:ILogger.cs
 *  Copyright (C) 2014 Example
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Infrastructure.Logging
{
    public interface ILogger
    {
        void Debug(string message, Exception exception);
        void Debug(Exception exception);
        void Error(string message, Exception exception);
        void Error(Exception exception);
        void Info(string message, Exception exception);
        void Info(Exception exception);
        void Warn(string message, Exception exception);
        void Warn(Exception exception);
    }
}
