#RPC

## 发起方
继承 ServiceContext

```
delegate void Method(int source, int session, string method, byte[] param);
delegate void RPCCallback(SSContext context, string method, byte[] param, RPCError error);
delegate void TimeoutCallback(SSContext context, long currentTime);
```


```
Send(int destination, string method, byte[] param)
Call(int destination, string method, byte[] param, SSContext context, RPCCallback cb)
```

- Send是直接往往目标消息队列插消息
- Call则是加多一个context, 及一个自增的session, 另外还注册回调Dict<session, cb>, 当服务端完成操作后, 
  DoResponse往来源消息队列插消息, worker从队列取消息执行时, 根据session找出cb执行
  

##MSG

- 认证
```json
{
  "ct": 1,
  "mt": 1,
  "op": 1,
  "data": {
    "username": "dwx",
    "password": "dwx666"
  }
}
```

- 选择角色
```json
{
  "ct": 1,
  "mt": 1,
  "op": 1,
  "data": {
    "username": "dwx",
    "password": "dwx666"
  }
}
```


## Data/UserName
- User.json
```json
{
  "UserName": "dwx",
  "PassWord": "dwx666",
  "Birthday": "2022-04-01 09:48:00"
}
```

## Data/UserName/ID
### UserID:100

## Data/UserName/ID/RoleAttr.json
## Data/UserName/ID/BB.json


