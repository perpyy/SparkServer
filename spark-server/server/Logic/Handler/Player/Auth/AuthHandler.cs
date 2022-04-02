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

        public static void Handler(LoopService loop, DispatchData data, ReqOp op, string msg)
        {
            switch (op)
            {
                case ReqOp.Player_Auth_Login:
                    var m = JsonConvert.DeserializeObject<ReqLogin>(msg);
                    LoggerHelper.Debug(m_serviceAddress, $"{m.Data.Username}, {m.Data.Password}");
                    break;
                case ReqOp.Player_Auth_Logout:
                    break;
                case ReqOp.Player_Auth_Select:
                    break;
                case ReqOp.Player_Auth_Create:
                    break;
                case ReqOp.Player_Auth_Delete:
                    break;
                case ReqOp.Player_Auth_Reconnect:
                    break;
                default:
                    break;
            }
        }


        public void Play_Auth_Login_Cb()
        {
            
        }
    }
}