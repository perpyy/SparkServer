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
        public unsafe void Dispatch(int source, int session, string method, byte[] param)
        {
            var sw = new Stopwatch();
            DispatchData data = new DispatchData(param);
            sw.Start();
            // 提base64
            var b64 = Convert.FromBase64String(data.buffer);
            // 提前面三个short，用于识别如何分发
            var ct = -1;
            var mt = -1;
            var op = -1;
            fixed (byte* mp = b64)
            {
                var mpp = mp;
                ct = *(short*) mpp;
                *mpp = 0; // 作用为让json不处理这一字节的数据
                *(mpp + 1) = 0; // 作用为让json不处理这一字节的数据
                mpp += 2;
                mt = *(short*) mpp;
                *mpp = 0;
                *(mpp + 1) = 0;
                mpp += 2;
                op = *(short*) mpp;
                *mpp = 0;
                *(mpp + 1) = 0;
            }
            
            // 提字符串, 用于json反序列化, 3个short的控制不用管, 已设为0, json会忽略
            var m = Encoding.UTF8.GetString(b64);
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            switch ((ReqCt)ct)
            {
                case ReqCt.Player: // 玩家
                    switch ((ReqMt)mt)
                    {
                        case ReqMt.Player_Auth:
                            AuthService.Handler(this, data, (ReqOp)op, m);
                            // call db service check user
                            break;
                        default:
                            break;
                    }
                    break;
                case ReqCt.Gm:
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
        
        public unsafe void Send2Client<T>(long tcpObjectId, long connection, ResMsgHeader res, T t)
        {
            var r = JsonConvert.SerializeObject(t);
            var msg = Encoding.UTF8.GetBytes(r);
            var buf = new byte[6 + msg.Length];
            fixed (byte* b = buf, m = msg)
            {
                var bb = b;
                var mm = m;
                *bb = (byte)((short)res.Ct << 8 & 0xf0);
                bb++;
                *bb = (byte)((short)res.Ct >> 8 & 0x0f);
                bb++;
                *bb = (byte)((short)res.Mt << 8 & 0xf0);
                bb++;
                *bb = (byte)((short)res.Mt >> 8 & 0x0f);
                bb++;
                *bb = (byte)((short)res.Op << 8 & 0xf0);
                bb++;
                *bb = (byte)((short)res.Op >> 8 & 0x0f);
                bb++;
                while (mm - m < msg.Length)
                {
                    *bb = *mm;
                    mm++;
                    bb++;
                }
            }
            
            var buffList = new List<byte[]>
            {
                /*Encoding.UTF8.GetBytes(r)*/
                buf
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