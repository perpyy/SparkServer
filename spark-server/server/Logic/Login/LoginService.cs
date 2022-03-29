using System;
using System.Collections.Generic;
using Dapper;
using NLog.LayoutRenderers;
using SparkServer.Framework.Utility;
using SparkServer.Logic.Login.Entity;
using SparkServer.MySQL;

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
            NetSprotoType.SocketData data = new NetSprotoType.SocketData(param);
            
            Framework.MessageQueue.NetworkPacket message = new Framework.MessageQueue.NetworkPacket();
            
            message.Type = SparkServer.Framework.MessageQueue.SocketMessageType.DATA;
            message.TcpObjectId = session;
            message.ConnectionId = data.connection;

            var result = MySQLHelper.GetInstance().Conn.Query<Player>("select * from players");
            foreach (var r in result)
            {
                Console.WriteLine($"{r.Id}, {r.UserName}, {r.PassWord}, {r.RegisterTime}, {r.Lock}");
            }

            List<byte[]> buffList = new List<byte[]>();
            buffList.Add(Convert.FromBase64String(data.buffer));
            message.Buffers = buffList;
            Framework.MessageQueue.NetworkPacketQueue.GetInstance().Push(message);
        }
    }
}