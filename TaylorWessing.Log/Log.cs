using System;
using System.Configuration;
using log4net;
using log4net.Config;
namespace TaylorWessing.Log
{
    public class Log : ILog
    {
        log4net.ILog logger;

        public Log(Type type)
        {
            logger = LogManager.GetLogger(type);
        }
        public void Info(object message)
        {
            logger.Info(message);
        }
        public void Debug(object message)
        {
            logger.Debug(message);
        }
        public void Error(object message, Exception exception)
        {
            logger.Error(message, exception);
        }
        public void Fatal(object message)
        {
            logger.Fatal(message);
        }
    }
}
