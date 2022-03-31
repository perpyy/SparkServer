using System;

namespace SparkServer.Logic
{
    public class CustomId
    {
        private static CustomId _mCustomId;
        private int _id = 10000;
        public static CustomId GetInstance()
        {
            return _mCustomId ?? (_mCustomId = new CustomId());
        }

        public int GetId()
        {
            _id += 1;
            return _id;
        }
    }
}