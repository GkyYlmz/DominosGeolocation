using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DominosGeolocation.Logging
{
    public class Logger
    {
        public static Logger _dlog;

        public static Logger DLog
        {
            get
            {
                if (_dlog == null)
                    _dlog = new Logger("Logger");

                return _dlog;
            }
        }

        private ILog logger;
        public static void Configure(ILoggerRepository repo, string file)
        {
            ConfigureBase(repo, file);
        }

        private static void ConfigureBase(ILoggerRepository repo, string file)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(file));

            var config = log4netConfig["log4net"];
            XmlConfigurator.Configure(repo, config);
        }

        public ILog Log
        {
            get { return this.logger; }
        }

        private string loggerName;
        private string LoggerName
        {
            get { return loggerName; }
        }

        private Logger(string loggerName)
        {
            this.loggerName = loggerName;
            this.logger = LogManager.GetLogger(Assembly.GetEntryAssembly(), LoggerName);
        }

        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Info(string message, Exception ex)
        {
            Log.Info(message, ex);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            Log.Error(message, ex);
        }

        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Debug(string message, Exception ex)
        {
            Log.Debug(message, ex);
        }

        public void Fatal(string message)
        {
            Log.Fatal(message);
        }

        public void Fatal(string message, Exception ex)
        {
            Log.Fatal(message, ex);
        }

        public void Warn(string message)
        {
            Log.Warn(message);
        }

        public void Warn(string message, Exception ex)
        {
            Log.Warn(message, ex);
        }

        public bool IsDebugEnabled
        {
            get { return Log.IsDebugEnabled; }
        }
        public bool IsErrorEnabled
        {
            get { return Log.IsErrorEnabled; }
        }
        public bool IsFatalEnabled
        {
            get { return Log.IsFatalEnabled; }
        }
        public bool IsInfoEnabled
        {
            get { return Log.IsInfoEnabled; }
        }
        public bool IsWarnEnabled
        {
            get { return Log.IsWarnEnabled; }
        }
    }
}
