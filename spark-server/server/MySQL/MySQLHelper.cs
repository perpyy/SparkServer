using System;
using System.Data;
using System.Threading.Tasks;
using MySqlConnector;
using SparkServer.Framework.Utility;

namespace SparkServer.MySQL
{
    public class MySQLHelper
    {
        private static MySQLHelper _mInstance;
        public MySqlConnection Conn = new XyqDb().Conn;
        public static MySQLHelper GetInstance()
        {
            return _mInstance ?? (_mInstance = new MySQLHelper());
        }
    }

    public class XyqDb
    {
        public MySqlConnection Conn;
        public XyqDb()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "106.53.147.160",
                Port = 13306,
                UserID = "root",
                Password = "Zyt!@#jsb456",
                Database = "mhxy",
                Pooling = true,
                MinimumPoolSize = 10,
                MaximumPoolSize = 100,
                ConnectionLifeTime = 3000
            };
            Conn = new MySqlConnection(builder.ConnectionString);
            try
            {
                Conn.Open();
                LoggerHelper.Info(0, $"Connect to MySQL: {builder.ConnectionString} success");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}