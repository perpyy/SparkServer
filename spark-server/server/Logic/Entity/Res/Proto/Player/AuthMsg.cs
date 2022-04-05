using System.Collections.Generic;

namespace SparkServer.Logic.Entity.Res.Proto.Player
{
    public class AuthMsg: ResMsgBase
    {
        public MsgData Data;
        public struct MsgData
        {
            public List<Player> Players;
        }
        
        public struct Player
        {
            
        }
    }
}