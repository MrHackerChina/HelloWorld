using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserHandler : HanderBase
{
    /// <summary>
    /// 角色的网络消息处理类
    /// </summary>
    /// <param name="subCode"></param>
    /// <param name="value"></param>
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case UserCode.CREATE_SERS:
                createResponse((int)value);
                break;
            case UserCode.GET_INFO_SRES:
                getInfoResponse(value as UserDto);
                break;
            case UserCode.ONLINE_SERS:
                onlineResponse((int)value);
                break;
            default:
                break;
        }
    }
    private SocketMsg socketMsg = new SocketMsg();
    /// <summary>
    /// 获取角色信息
    /// </summary>
    /// <param name="user"></param>
    private void getInfoResponse(UserDto user)
    {
        if (user == null)
        {
            //没有角色
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, true);

        }
        else
        {
            // 有角色
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            //角色上线

            //socketMsg.Change(OpCode.USER, UserCode.ONLINE_CREQ, null);
            //Dispatch(AreaCode.NET, 0, socketMsg);

            Models.GameModel.UserDto = user;
            //更新数据
            Dispatch(AreaCode.UI, UIEvent.REFRESH_INFO_PANEL, user);
        }
    }

    /// <summary>
    /// 上线的响应
    /// </summary>
    /// <param name="result"></param>
    private void onlineResponse(int result)
    {
        if (result==0)
        {
            //success
            Debug.LogError("online success");
        } 
        else if(result==-1)
        {
            // fail 
            Debug.LogError("fail : 非法登录");
        }
        else if(result == -2)
        {
            Debug.LogError("fail : user is null");
        }
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="result"></param>
    private void createResponse(int result)
    {
        if(result==-1)
        {

        }
        else if(result==-2)
        {

        }
        else if(result==0)
        {
            Dispatch(AreaCode.UI, UIEvent.CREATE_PANEL_ACTIVE, false);
            socketMsg.Change(OpCode.USER, UserCode.GET_INFO_CREQ, null);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
    }
}

