namespace SparkServer.Logic.Entity.Req.Proto.Player
{
    public class ReqLogin: ReqMsgBase
    {
        public MsgData Data;
        public struct MsgData
        {
            public string Username;
            public string Password;
        }
    }
}