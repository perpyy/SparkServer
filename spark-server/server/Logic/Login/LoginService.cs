﻿using System;
using System.Collections.Generic;
using NetSprotoType;
using SparkServer.Framework.Utility;

namespace SparkServer.Logic.Login
{
    class LoginService : Framework.Service.ServiceContext
    {
        protected override void Init()
        {
            base.Init();
            RegisterServiceMethods("Login", Login);
        }

        public void Login(int source, int session, string method, byte[] param)
        {
            string text = $"Call method: {method}";
            LoggerHelper.Info(m_serviceAddress, text);
            DispatchData data = new DispatchData(param);
            
            Framework.MessageQueue.NetworkPacket message = new Framework.MessageQueue.NetworkPacket();
            
            message.Type = SparkServer.Framework.MessageQueue.SocketMessageType.DATA;
            message.TcpObjectId = (int)data.tcpObjectId;
            message.ConnectionId = data.connection;
            

            List<byte[]> buffList = new List<byte[]>();
            buffList.Add(Convert.FromBase64String(data.buffer));
            message.Buffers = buffList;
            Framework.MessageQueue.NetworkPacketQueue.GetInstance().Push(message);
        }
    }
}