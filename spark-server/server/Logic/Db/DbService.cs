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
            var bootConf = SparkServerUtility.GetBootConf();
            var bootConfigText = ConfigHelper.LoadFromFile(bootConf);
            var configJson = JObject.Parse(bootConfigText);
            if (!configJson.ContainsKey("Db"))
            {
                throw new Exception("Config file does not has Db field");
            }

            var db = configJson["Db"].ToString();
            if (!Directory.Exists(db)) throw new Exception("Db does not exits");
            var directoryInfo = new DirectoryInfo(db);
            // 取DB底下所有目录，目录代码账号
            var dirs = directoryInfo.GetDirectories();
            foreach (var d in dirs)
            {
                LoadPlayerInfo(d);
            }

        }

        private static void LoadPlayerInfo(DirectoryInfo player)
        {
            var f = System.IO.Path.Combine(player.FullName, "user.json");
            var user = File.ReadAllText(f);
            var p = JsonConvert.DeserializeObject<EtPlayer>(user);
        }
        
        
    }
}