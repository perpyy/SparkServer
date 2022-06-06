using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NetSprotoType;
using Newtonsoft.Json;
using SparkServer.Framework.Service;
using SparkServer.Framework.Utility;
using SparkServer.Logic.Db;
using SparkServer.Logic.Entity;
using SparkServer.Logic.Entity.Player;
using SparkServer.Logic.Entity.Req;
using SparkServer.Logic.Entity.Req.MsgType;
using SparkServer.Logic.Entity.Res;
using SparkServer.Logic.Handler.Player.Auth;
using SparkServer.Network;

namespace SparkServer.Logic.Loop
{
    /*
     * Game主逻辑
     */
    class LoopService: ServiceContext
    {
        
        /*
         * 定义所有的全局数据
         */

        public Dictionary<string, EtPlayer> GPlayers;
        
        public Dictionary<long, string> OnLinePlayer;
        protected override void Init()
        {
            base.Init();
            /*
             * 初始化所有的全局数据
             */

            GPlayers = new Dictionary<string, EtPlayer>();
            OnLinePlayer = new Dictionary<long, string>();

            /*
             * 从盘里读所有的数据
             */
            DbService.Load(this);
            RegisterServiceMethods(nameof(Dispatch), Dispatch);
        }

        // 分发
        public void Dispatch(int source, int session, string method, byte[] param)
        {
            var sw = new Stopwatch();
            DispatchData data = new DispatchData(param);

            // 反序列为JSON,只反操作码部分，用于识别如何分发
            var m = Encoding.UTF8.GetString(Convert.FromBase64String(data.buffer));
            // 先提操作码
            sw.Start();

            var op = JsonConvert.DeserializeObject<ReqMsgHeader>(m);
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            switch (op.Ct)
            {
                case ReqCt.Player: // 玩家
                    switch (op.Mt)
                    {
                        case ReqMt.Player_Auth:
                            AuthService.Handler(this, data, op.Op, m);
                            // call db service check user
                            break;
                        default:
                            break;
                    }
                    break;
                case ReqCt.Gm: // GM
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            /*Send2Client(data.tcpObjectId, data.connection, data.buffer);*/
        }





        /*
         * DbService读用户账号信息后会回调这里传到这个线程（Service）
         */
        public void LoadPlayerAccountCb(SSContext context, string method, byte[] param, RPCError error)
        {
            
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

        public void Send2Client<T>(long tcpObjectId, long connection, T t)
        {
            var r = JsonConvert.SerializeObject(t);
            Send2Client(tcpObjectId, connection, r);
        }
        
        public void Send2Client<T>(long tcpObjectId, long connection, ResMsgHeader res, T t)
        {
            var r = JsonConvert.SerializeObject(t);
            var buffList = new List<byte[]>
            {
                Encoding.UTF8.GetBytes(r)
            };
            Send2Client(tcpObjectId, connection, buffList);
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