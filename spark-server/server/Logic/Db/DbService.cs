using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SparkServer.Framework.Service;
using SparkServer.Framework.Utility;
using SparkServer.Logic.Entity.Player;
using SparkServer.Logic.Loop;

namespace SparkServer.Logic.Db
{
    /*
     * 读盘
     * 存盘
     */
    class DbService: ServiceContext
    {
        protected override void Init()
        {
            base.Init();
            RegisterServiceMethods(nameof(Dump), Dump); // Dump
        }

        /*
         * 存盘
         */
        public void Dump(int source, int session, string method, byte[] param)
        {
            
        }
        
        /*
         * 读盘
         */
        public static void Load(LoopService loop)
        {
            // 取配置文件路径
            var bootConf = SparkServerUtility.GetBootConf();
            var bootConfigText = ConfigHelper.LoadFromFile(bootConf);
            var configJson = JObject.Parse(bootConfigText);
            // 检查是否有Db字段
            if (!configJson.ContainsKey("Db"))
            {
                throw new Exception("Config file does not has Db field");
            }
            var db = configJson["Db"].ToString();
            if (!Directory.Exists(db)) throw new Exception("Db does not exits");
            var directoryInfo = new DirectoryInfo(db);
            // 取DB底下所有目录，目录代表账号
            var dirs = directoryInfo.GetDirectories();
            // 读取目录里面的：用户数据
            foreach (var d in dirs)
            {
                LoadPlayerInfo(loop, d);
            }
        }

        private static void LoadPlayerInfo(LoopService loop, DirectoryInfo player)
        {
            var f = Path.Combine(player.FullName, "user.json");
            var user = File.ReadAllText(f);
            var p = JsonConvert.DeserializeObject<EtPlayer>(user);
            // 往全局数据Add
            loop.GPlayers.Add(p.Username, p);
        }
        
        
    }
}