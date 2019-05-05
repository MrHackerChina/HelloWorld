using Protocol;
using Protocol.Code;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AccoutHander : HanderBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.LOGIN:
                loginResponse((int)value);
                break;
            case AccountCode.REGIST_SRES:
                registResponse((int)value);
                break;
            default:
                break;
        }
    }
    private PromptMsg prompMsg = new PromptMsg();
    /// <summary>
    /// 登录
    /// </summary>
    private void loginResponse(int result)
    {
        if(int.Equals(result,0))
        {
            LoadSceneMsg msg = new LoadSceneMsg(1, () =>
            {
                //向服务器获取用户信息
                SocketMsg socktMsg = new SocketMsg(OpCode.USER, UserCode.GET_INFO_CREQ, null);
                Dispatch(AreaCode.NET, 0, socktMsg);
                Debug.Log("进入游戏场景");
            });
            Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, msg);
            return;
        }

        switch (result)
        {
            case -1:
                prompMsg.Change("账号不存在", Color.red);
                break;
            case -2:
                prompMsg.Change("账号 & 密码 错误", Color.red);
                break;
            case -3:
                prompMsg.Change("账号 & 密码 异地登录", Color.red);
                break;
            default:
                break;
        }
        Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, prompMsg);
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="value"></param>
    private void registResponse(int result)
    {
        switch (result)
        {
            case 0:
                prompMsg.Change("注册成功", Color.green);
                break;
            case -1:
                prompMsg.Change("账号已经存在", Color.red);
                break;
            case -2:
                prompMsg.Change("账号输入不合法", Color.red);
                break;
            case -3:
                prompMsg.Change("密码不合法", Color.red);
                break;
            default:
                break;
        }
        Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, prompMsg);
    }

}

