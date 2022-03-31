using System;
using System.Collections.Generic;
using NetSprotoType;
using Newtonsoft.Json;
using SparkServer.Framework.Service;
using SparkServer.Framework.Service.Logger;
using SparkServer.Framework.Utility;
using SparkServer.Logic.Entity.Req.MsgType;
using SparkServer.Logic.Entity.Req.Proto.Player;
using SparkServer.Logic.Loop;

namespace SparkServer.Logic.Handler.Player.Auth
{
    class AuthService
    {
        private static int m_serviceAddress = CustomId.GetInstance().GetId();
        private static int _id = LoggerHelper.AddService(m_serviceAddress, nameof(AuthService));

        public static void Handler(LoopService loop, ReqOp op, string msg)
        {
            switch (op)
            {
                case ReqOp.Player_Auth_Login:
                    var m = JsonConvert.DeserializeObject<ReqLogin>(msg);
                    LoggerHelper.Debug(m_serviceAddress, $"{m.Data.UserName}, {m.Data.PassWord}");
                    break;
                default:
                    break;
            }
        }
    }
}