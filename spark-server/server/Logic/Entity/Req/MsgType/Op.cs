namespace SparkServer.Logic.Entity.Req.MsgType
{
    /*
     * Auth相关
     */
    public enum ReqOp
    {
        Player_Auth_Login, // 登录
        Player_Auth_Logout, // 退出
        Player_Auth_Select, // 选择角色
        Player_Auth_Create, // 创建角色
        Player_Auth_Delete, // 删除角色
        Player_Auth_Reconnect, // 重连
    }
}