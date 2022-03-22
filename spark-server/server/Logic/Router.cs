﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparkServer.Framework.Service;
using SparkServer.Framework.Utility;

namespace SparkServer.Logic
{
    class Router : SparkServer.Framework.Service.Gateway.Gateway
    {
        protected override void Init()
        {
            base.Init();
        }

        protected override void SocketAccept(int source, int session, string method, byte[] param)
        {
            LoggerHelper.Info(m_serviceAddress, "GatewayCase.SocketAccept");
        }

        protected override void SocketError(int source, int session, string method, byte[] param)
        {
            LoggerHelper.Info(m_serviceAddress, "GatewayCase.SocketError");
        }

        protected override void SocketData(int source, int session, string method, byte[] param)
        {
            NetSprotoType.SocketData data = new NetSprotoType.SocketData(param);

            LoggerHelper.Info(m_serviceAddress, String.Format("GatewayCase.SocketData:{0},{1}", data.connection, data.buffer));


            Message msg = new Message();
            msg.Source = GetId();
            msg.Type = MessageType.ServiceRequest;
            msg.Method = "Login";
            msg.Destination = ServiceSlots.GetInstance().Name2Id("LoginService");
            msg.Data = param;
            msg.RPCSession = this.GetTcpObjectId();
            ServiceSlots.GetInstance().Get(msg.Destination).Push(msg);
            


            /*Framework.MessageQueue.NetworkPacket message = new Framework.MessageQueue.NetworkPacket();
            message.Type = SparkServer.Framework.MessageQueue.SocketMessageType.DATA;
            message.TcpObjectId = this.GetTcpObjectId();
            message.ConnectionId = data.connection;

            List<byte[]> buffList = new List<byte[]>();
            buffList.Add(Convert.FromBase64String(data.buffer));
            message.Buffers = buffList;

            Framework.MessageQueue.NetworkPacketQueue.GetInstance().Push(message);*/
        }
    }
}