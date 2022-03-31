using System;
using System.Collections.Generic;
using System.Text;
using NetSprotoType;
using Newtonsoft.Json;
using SparkServer.Framework.Service;
using SparkServer.Framework.Utility;
using SparkServer.Logic.Entity;
using SparkServer.Logic.Entity.Req;
using SparkServer.Logic.Entity.Req.MsgType;
using SparkServer.Logic.Handler.Player.Auth;

namespace SparkServer.Logic.Loop
{
    /*
     * Game主逻辑
     */
    class LoopService: ServiceContext
    {
        protected override void Init()
        {
            base.Init();
            RegisterServiceMethods(nameof(Dispatch), Dispatch);
        }

        // 分发
        public void Dispatch(int source, int session, string method, byte[] param)
        {
            DispatchData data = new DispatchData(param);
            
            // 反序列为JSON,只反操作码部分，用于识别如何分发
            var m = Encoding.UTF8.GetString(Convert.FromBase64String(data.buffer));
            // 先提操作码
            var op = JsonConvert.DeserializeObject<ReqMsgBase>(m);

            switch (op.Mt)
            {
                case ReqMt.Player_Auth:
                    AuthService.Handler(this, op.Op, m);
                    break;
                default:
                    break;
            }
            /*Send2Client(data.tcpObjectId, data.connection, data.buffer);*/
        }

        private void Send2Client(long tcpObjectId, long connection, List<byte[]> msg)
        {
            Framework.MessageQueue.NetworkPacket message = new Framework.MessageQueue.NetworkPacket();
            
            message.Type = SparkServer.Framework.MessageQueue.SocketMessageType.DATA;
            message.TcpObjectId = (int)tcpObjectId;
            message.ConnectionId = connection;
            
            message.Buffers = msg;
            Framework.MessageQueue.NetworkPacketQueue.GetInstance().Push(message);
        }

        private void Send2Client(long tcpObjectId, long connection, string msg)
        {
            var buffList = new List<byte[]>
            {
                Encoding.UTF8.GetBytes(msg)
            };
            Send2Client(tcpObjectId, connection, buffList);
        }
    }
}