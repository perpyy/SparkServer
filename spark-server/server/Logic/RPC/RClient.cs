using SparkServer.Framework.Service;
using SparkServer.Framework.Utility;

namespace SparkServer.Logic.RPC
{
    class RClient: ServiceContext
    {
        protected override void Init()
        {
            base.Init();
            Timeout(null, 1, SendGetUserInfo);
        }

        private void SendGetUserInfo(SSContext context, long t)
        {
            LoggerHelper.Info(m_serviceAddress, "Send RPC call");
            var c = new SSContext();
            c.IntegerDict["sessionID"] = 1;
            Call("RServer", "GetUserInfo", new byte[]{}, c, SendGetUserInfoCb);
        }

        private void SendGetUserInfoCb(SSContext context, string method, byte[] param, RPCError error)
        {
            LoggerHelper.Info(m_serviceAddress, context.IntegerDict["sessionID"].ToString());
            LoggerHelper.Info(m_serviceAddress, "cb");
        }
    }
}