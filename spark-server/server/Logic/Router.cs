using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSprotoType;
using SparkServer.Framework.Service;
using SparkServer.Framework.Utility;
using SparkServer.Logic.Login;
using SparkServer.Network;

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
            SocketAccept accept = new SocketAccept(param);
            LoggerHelper.Info(m_serviceAddress, $"SocketAccept {accept.ip}:{accept.port}, Connection Id: {accept.connection}");
        }

        protected override void SocketError(int source, int session, string method, byte[] param)
        {
            SocketError sprotoSocketError = new SocketError(param);
            switch (sprotoSocketError.errorCode)
            {
                case (int)SessionSocketError.Disconnected:
                    LoggerHelper.Info(m_serviceAddress, $"SocketDisconnected {sprotoSocketError.remoteEndPoint}, Connection Id: {sprotoSocketError.connection}");
                    break;
                case (int)ConnectionStatus.Disconnecting:
                    LoggerHelper.Info(m_serviceAddress, $"SocketDisconnecting {sprotoSocketError.remoteEndPoint}, Connection Id: {sprotoSocketError.connection}");
                    break;
                default:
                    break;
            }
        }

        protected override void SocketData(int source, int session, string method, byte[] param)
        {
            NetSprotoType.SocketData data = new NetSprotoType.SocketData(param);

            LoggerHelper.Info(m_serviceAddress, $"Receive data from connection id: {data.connection}");
            
            // 提取消息类型
            // GetTcpObjectId() 在这个service里面即GetId();
            // 分发到各个服务

            Message msg = new Message();
            msg.Source = GetId();
            msg.Type = MessageType.ServiceRequest;
            msg.Method = nameof(LoginService.Login);
            msg.Destination = ServiceSlots.GetInstance().Name2Id(nameof(LoginService));
            msg.Data = param;
            msg.RPCSession = GetTcpObjectId();
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