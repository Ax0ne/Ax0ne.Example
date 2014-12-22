/*----------------------------*\
 *  Author:Ax0ne
 *  CreateTime:2014/9/3 10:40:29
 *  FileName:Logger.cs
 *  Copyright (C) 2014 Sumsz.IT
\*----------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile="log4net.config",Watch=true)]
namespace Example.Infrastructure.Logging
{
    public class Logger:ILogger
    {
        public static readonly ILogger Instance = new Logger();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Ax0ne.Log4net");
        public void Debug(string message, Exception exception)
        {
            log.Debug(message, exception);
        }

        public void Debug(Exception exception)
        {
            log.Debug(exception);
        }

        public void Error(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void Error(Exception exception)
        {
            log.Error(exception);
        }

        public void Info(string message, Exception exception)
        {
            log.Info(message, exception);
        }

        public void Info(Exception exception)
        {
            log.Info(exception);
        }

        public void Warn(string message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public void Warn(Exception exception)
        {
            log.Warn(exception);
        }
    }
}
