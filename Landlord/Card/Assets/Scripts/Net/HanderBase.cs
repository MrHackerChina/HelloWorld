using System;
using System.Collections.Generic;

/// <summary>
/// 客户端的请求
/// </summary>
public abstract class HanderBase
{
    public abstract void OnReceive(int subCode,object value);


    protected void Dispatch(int areaCode,int subCode,object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, subCode, message);
    }
}

