﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparkServer.Framework.Service;
using SparkServer.Framework.Service.Logger;

namespace SparkServer.Framework.Utility
{
    class LoggerHelper
    {
        public static void Info(int source, string msg)
        {
            string logger = "LoggerService";
            LoggerService loggerService = (LoggerService)ServiceSlots.GetInstance().Get(logger);

            Message message = new Message();
            message.Method = "OnLog";
            message.Data = Encoding.ASCII.GetBytes(msg);
            message.Destination = loggerService.GetId();
            message.Source = source;
            message.Type = MessageType.ServiceRequest;
            loggerService.Push(message);
        }
    }
}
