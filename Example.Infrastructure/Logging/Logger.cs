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
        private static readonly ILog _log = LogManager.GetLogger("Ax0ne.Log4net");
        public void Debug(string message, Exception exception)
        {
            _log.Debug(message, exception);
        }

        public void Debug(Exception exception)
        {
            _log.Debug(exception);
        }

        public void Error(string message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Error(Exception exception)
        {
            _log.Error(exception);
        }

        public void Info(string message, Exception exception)
        {
            _log.Info(message, exception);
        }

        public void Info(Exception exception)
        {
            _log.Info(exception);
        }

        public void Warn(string message, Exception exception)
        {
            _log.Warn(message, exception);
        }

        public void Warn(Exception exception)
        {
            _log.Warn(exception);
        }
    }
}
