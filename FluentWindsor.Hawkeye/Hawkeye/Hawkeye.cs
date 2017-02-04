using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using log4net;
using log4net.Config;

namespace FluentlyWindsor.Hawkeye
{
    public class Hawkeye : IInterceptor
    {
        private static bool isConfigured;

        public virtual void Intercept(IInvocation invocation)
        {
            if (!isConfigured)
                ConfigureLog4Net();

            Exception ex = null;
            Stopwatch sw = null;
            var log = LogManager.GetLogger(invocation.TargetType);

            try
            {
                sw = Stopwatch.StartNew();
                invocation.Proceed();
            }
            catch (Exception error)
            {
                ex = error;
            }
            finally
            {
                sw.Stop();
            }

            Task.Factory.StartNew(() =>
            {
                var @params = new LoggingFormatterParams(invocation, sw.Elapsed, ex);
                ExecuteLoggingAction(invocation, @params, log);
            });

            if (ex != null)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }

                throw ex;
            }
        }

        protected virtual void ExecuteLoggingAction(IInvocation invocation, LoggingFormatterParams @params, ILog log)
        {
            try
            {
                var levelTargetConfiguration =
                    new Dictionary<LogLevel, Action<string, Exception>>
                        {
                            {LogLevel.Debug, log.Debug},
                            {LogLevel.Error, log.Error},
                            {LogLevel.Fatal, log.Fatal},
                            {LogLevel.Info, log.Info},
                            {LogLevel.Warn, log.Warn},
                        };

                var attributes = GetLogAttributes(invocation);
                if (attributes.Length > 0)
                {
                    var logAttribute = attributes.First();

                    Action<string, Exception> logAction;
                    if (@params.Exception == null)
                        logAction = levelTargetConfiguration[logAttribute.LogLevel];
                    else
                        logAction = levelTargetConfiguration[LogLevel.Error];

                    LogDetails(logAction, logAttribute, @params);
                }
            }
            catch(Exception err)
            {
                log.Error("Logger Broke!", err);
            }
        }

        private static void LogDetails(Action<string, Exception> logTarget, LogAttribute attribute, LoggingFormatterParams @params)
        {
            if (attribute.Formatter != null)
            {
                var logFormat = attribute.Formatter.GetLogFormat(@params, attribute);
                logTarget(logFormat.Take(4096).Aggregate("", (input, next) => input += next), null);
            }
            else
                logTarget(@params.Invocation.MethodInvocationTarget.Name, @params.Exception);
        }

        private static LogAttribute[] GetLogAttributes(IInvocation invocation)
        {
            var attributes =
                invocation
                    .TargetType
                    .GetCustomAttributes(typeof (LogAttribute), inherit: true)
                    .Cast<LogAttribute>()
                    .ToArray();

            if (invocation.Method != null)
                attributes =
                    attributes
                        .Concat(
                            invocation
                                .MethodInvocationTarget
                                .GetCustomAttributes(typeof (LogAttribute), inherit: true)
                                .Cast<LogAttribute>())
                        .ToArray();

            return attributes.Reverse().ToArray();
        }

        private static void ConfigureLog4Net()
        {
            var configurationFile = new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            XmlConfigurator.ConfigureAndWatch(configurationFile);
            isConfigured = true;
        }
    }
}