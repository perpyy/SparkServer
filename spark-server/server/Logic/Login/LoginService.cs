using System;
using System.Collections.Generic;
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

        private void Login(int source, int session, string method, byte[] param)
        {
            string text = string.Format("Service:{0} has been call method {1}", m_serviceAddress, method);
            LoggerHelper.Info(m_serviceAddress, text);
            NetSprotoType.SocketData data = new NetSprotoType.SocketData(param);
            
            Framework.MessageQueue.NetworkPacket message = new Framework.MessageQueue.NetworkPacket();
            
            message.Type = SparkServer.Framework.MessageQueue.SocketMessageType.DATA;
            message.TcpObjectId = session;
            message.ConnectionId = data.connection;

            List<byte[]> buffList = new List<byte[]>();
            buffList.Add(Convert.FromBase64String(data.buffer));
            message.Buffers = buffList;
            Framework.MessageQueue.NetworkPacketQueue.GetInstance().Push(message);
        }
    }
}