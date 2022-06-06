using System;
using System.Collections.Generic;
using NetSprotoType;
using Newtonsoft.Json;
using SparkServer.Framework.Service;
using SparkServer.Framework.Service.Logger;
using SparkServer.Framework.Utility;
using SparkServer.Logic.Entity.Player;
using SparkServer.Logic.Entity.Req.MsgType;
using SparkServer.Logic.Entity.Req.Proto.Player;
using SparkServer.Logic.Entity.Res;
using SparkServer.Logic.Entity.Res.MsgType;
using SparkServer.Logic.Entity.Res.Proto.Player;
using SparkServer.Logic.Loop;

namespace SparkServer.Logic.Handler.Player.Auth
{
    class AuthService
    {
        private static int m_serviceAddress = CustomId.GetInstance().GetId();
        private static int _id = LoggerHelper.AddService(m_serviceAddress, nameof(AuthService));

        // loopService带有全局变量, dspData带有连接信息, op带有操作码, msg带的是客户端请求的真正数据
        public static void Handler(LoopService loop, DispatchData dspData, ReqOp op, string msg)
        {
            switch (op)
            {
                case ReqOp.Player_Auth_Login:
                    var m = JsonConvert.DeserializeObject<ReqLogin>(msg);
                    LoggerHelper.Debug(m_serviceAddress, $"{m.Username}, {m.Password}");
                    // 验证账号密码
                    // 有这个账号
                    if (loop.GPlayers.TryGetValue(m.Username, out var et) && et.Password.Equals(m.Password))
                    {
                        // 账号密码对, 加入在线用户
                        loop.OnLinePlayer.Add(dspData.connection, m.Username);
                        // 发送成功登录信息，跳转角色列表界面
                        ResMsgHeader header = new ResMsgHeader()
                        {
                            Ct = ResCt.Player,
                            Mt = ResMt.Player_Auth,
                            Op = ResOp.Player_Auth_List,
                        };
                        AuthMsg resMsg = new AuthMsg()
                        {
                            Players = new List<AuthMsg.Player>()
                        };
                        loop.Send2Client<AuthMsg>(dspData.tcpObjectId, dspData.connection, header, resMsg);
                    }
                    else
                    {
                        // 发送错误信息
                    }

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