using SparkServer.Framework.Service;
using SparkServer.Framework.Utility;

namespace SparkServer.Logic.RPC
{
    class RServer: ServiceContext
    {
        protected override void Init()
        {
            base.Init();
            RegisterServiceMethods(nameof(GetUserInfo), GetUserInfo);
        }

        private void GetUserInfo(int source, int session, string method, byte[] param)
        {
            LoggerHelper.Info(m_serviceAddress, "Call method GetUserInfo");
            DoResponse(source, "SendGetUserInfoCb", new byte[]{}, session);
        }
    }
}