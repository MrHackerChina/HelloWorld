using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol.Code;
using Protocol.Dto;

public class MatchHandler : HanderBase
{

    private PromptMsg promptMsg = new PromptMsg();

    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case MatchCode.ENTER_SERS:
                enterResponse(value as MatchRoomDto);
                break;
            case MatchCode.ENTER_BRO:
                enterBro(value as UserDto);
                break;
            case MatchCode.LEAVE_BRO:
                leaverBro((int)value);
                break;
            case MatchCode.START_BRO:
                startBro();
                break;
            case MatchCode.READY_BRO:
                readyBro((int)value);
                break;
            default:
                break;
        }
    }

  
    /// <summary>
    /// 自己进入房间
    /// </summary>
    /// <param name="matchRoom"></param>
    private void enterResponse(MatchRoomDto room)
    {
        //GameModel gModel = Models.GameModel;
        //MatchRoomDto matchRoom = gModel.MatchRoomDto;
        //int myUserID = gModel.UserDto.Id;
        //matchRoom = room;
        //gModel.MatchRoomDto.ResetPosition(myUserID);

        //更新角色数据
        //if(matchRoom.LeftId!=-1)
        //{
        //    UserDto leftUserDto = matchRoom.UIdUserDict[matchRoom.LeftId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, leftUserDto);
        //}
        //if(matchRoom.RightId!=-1)
        //{
        //    UserDto rightUserDto = matchRoom.UIdUserDict[matchRoom.RightId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, rightUserDto);
        //}

        Models.GameModel.MatchRoomDto = room;
        resetPositon();
        /*
        int myUserID = Models.GameModel.UserDto.Id;
        //自身角色
        UserDto myUserDto = Models.GameModel.MatchRoomDto.UIdUserDict[myUserID];
        //发送
        Dispatch(AreaCode.UI, UIEvent.SET_MY_PLAYER_DARA, myUserDto);
        */
        //显示进入房间的按钮
        Dispatch(AreaCode.UI, UIEvent.SHOW_ENTER_ROOM_BUTTON, null);
    }
    /// <summary>
    /// 其他角色进入的广播
    /// </summary>
    /// <param name="userDto"></param>
    private void enterBro(UserDto newUser)
    {
        //Dispatch(AreaCode.UI, UIEvent.PLAYER_ENTER, newUser.Id);
        MatchRoomDto matchRoom = Models.GameModel.MatchRoomDto;
        matchRoom.Add(newUser);
        resetPositon();

        if (matchRoom.LeftId != -1)
        {
            UserDto leftUserDto = matchRoom.UIdUserDict[matchRoom.LeftId];
            Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, leftUserDto);
        }
        if (matchRoom.RightId != -1)
        {
            UserDto rightUserDto = matchRoom.UIdUserDict[matchRoom.RightId];
            Dispatch(AreaCode.UI, UIEvent.SET_RIGHT_PLAYER_DATA, rightUserDto);
        }

        Dispatch(AreaCode.UI, UIEvent.PLAYER_ENTER, newUser.Id);
        //TODO
        promptMsg.Change("有新玩家进入（" + newUser.Id + ")进入", Color.blue);
        Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
    }

    /// <summary>
    /// 重置位置
    /// </summary>
    private void resetPositon()
    {
        GameModel gModel = Models.GameModel;
        MatchRoomDto matchRoom = gModel.MatchRoomDto;
        matchRoom.ResetPosition(gModel.UserDto.Id);

        //if (matchRoom.LeftId != -1)
        //{
        //    UserDto leftUserDto = matchRoom.UIdUserDict[matchRoom.LeftId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, leftUserDto);
        //}
        //if (matchRoom.RightId != -1)
        //{
        //    UserDto rightUserDto = matchRoom.UIdUserDict[matchRoom.RightId];
        //    Dispatch(AreaCode.UI, UIEvent.SET_LEFT_PLAYER_DATA, rightUserDto);
        //}
    }
    /// <summary>
    /// 离开
    /// </summary>
    /// <param name="leaverUserId"></param>
    private void leaverBro(int leaverUserId)
    {
        // 更新数据 TODO
        Dispatch(AreaCode.UI, UIEvent.PLAYER_LEAVE, leaverUserId);
        resetPositon();
        Models.GameModel.MatchRoomDto.Leaver(leaverUserId);
    }

    /// <summary>
    /// 准备
    /// </summary>
    /// <param name="readyUserId"></param>
    private void readyBro(int readyUserId)
    {
        Models.GameModel.MatchRoomDto.Ready(readyUserId);

        Dispatch(AreaCode.UI, UIEvent.PLAYER_READY, readyUserId);

        if (readyUserId == Models.GameModel.UserDto.Id)
            Dispatch(AreaCode.UI, UIEvent.PLAY_HIDE_READ_BUTTON, null);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void startBro()
    {
        promptMsg.Change("所有玩家准备开始游戏",Color.blue);
        Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);

        Dispatch(AreaCode.UI, UIEvent.PLAYER_HIDE_SETATE, null);
    }
}
