using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSprotoType;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SparkServer.Framework.Service.Logger
{
    class LoggerService : ServiceContext
    {
        private NLog.Logger m_logger; // = LogManager.GetCurrentClassLogger();
        private Dictionary<int, string> _serviceId2Name;

        protected override void Init(byte[] param)
        {
            base.Init();

            Logger_Init loggerInit = new Logger_Init(param);
            Startup(loggerInit.logger_path);

            RegisterServiceMethods("OnLog", OnLog);
            _serviceId2Name = new Dictionary<int, string> {{0, "SparkServer"}};

        }

        private void Startup(string loggerPath)
        {
            // 配置参见 https://github.com/nlog/nlog/wiki/Configuration-API

            var config = new LoggingConfiguration();

            var logRoot = loggerPath;

            var filePrefix = "log_";

            var fileTarget = new FileTarget("target")
            {
                FileName = logRoot + "${shortdate}/" + filePrefix + "${date:universalTime=false:format=yyyy_MM_dd_HH}.log",
                Layout = "${longdate} ${level} ${message}",
                KeepFileOpen = true,
                AutoFlush = true,
            };
            var consoleTarget = new ConsoleTarget("console")
            {
                Layout = "${longdate} ${level} ${message}"
            };
            config.AddTarget(fileTarget);
            config.AddTarget(consoleTarget);

            config.AddRuleForAllLevels(fileTarget);
            config.AddRuleForAllLevels(consoleTarget);

            LogManager.Configuration = config;

            m_logger = LogManager.GetCurrentClassLogger();
        }

        private void OnLog(int source, int session, string method, byte[] param)
        {
            var service = "";
            if (_serviceId2Name.ContainsKey(source))
            {
                service = _serviceId2Name[source];
            }
            else
            {
                service = ServiceSlots.GetInstance().Id2Name(source);
                _serviceId2Name.Add(source, service);
            }
            string outStr = string.Format("[{0:X8}] [{1, -20}] {2}", source, service, Encoding.ASCII.GetString(param));
            m_logger.Info(outStr);
        }
    }
}