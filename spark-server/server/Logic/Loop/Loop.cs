using SparkServer.Framework.Service;

namespace SparkServer.Logic.Loop
{
    /*
     * Game主逻辑
     */
    class Loop: ServiceContext
    {
        protected override void Init()
        {
            base.Init();
        }

        // 分发
        private void Dispatch(int source, int session, string method, byte[] param)
        {
            
        }
    }
}