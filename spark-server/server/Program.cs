using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SparkServer.Network;
using SparkServer.Framework;
using SparkServer.Framework.Utility;
using SparkServer.Logic;

namespace SparkServer
{
    class Program
    {
        static void Main(string[] args)
        {
            void BootServices()
            {
                // 这里启动各类服务
                SparkServerUtility.NewService("SparkServer.Logic.Login.LoginService", "LoginService");
                SparkServerUtility.NewService("SparkServer.Logic.RPC.RServer", "RServer");
                SparkServerUtility.NewService("SparkServer.Logic.RPC.RClient", "RClient");
            }
            
            var server = new Server();
            server.Run("../../Logic/config/start.json", BootServices);
        }
    }
}
