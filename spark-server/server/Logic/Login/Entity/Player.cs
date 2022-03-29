using System;

namespace SparkServer.Logic.Login.Entity
{
    public class Player
    {
        public int Id;
        public string UserName;
        public string PassWord;
        public DateTime RegisterTime;
        public bool Lock;
    }
}