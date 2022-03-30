using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SparkServer.Framework.Utility
{
    class ConfigHelper
    {
        public static string LoadFromFile(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : "";
        }
    }
}
