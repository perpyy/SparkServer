using SparkServer.Framework;
using SparkServer.Framework.Utility;

namespace SparkServer
{
    class Program
    {
        static void Main(string[] args)
        {
            void BootServices()
            {
                // 这里启动各类服务
                SparkServerUtility.NewService("SparkServer.Logic.Db.DbService", "DbService");
                /*SparkServerUtility.NewService("SparkServer.Logic.RPC.RServer", "RServer");
                SparkServerUtility.NewService("SparkServer.Logic.RPC.RClient", "RClient");*/
                SparkServerUtility.NewService("SparkServer.Logic.Loop.LoopService", "LoopService");
            }
            
            var server = new Server();
            server.Run("Config/start.json", BootServices);
        }
    }
}
