using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIEvent
{
    /// <summary>
    /// 设置开始面板显示
    /// </summary>
    public const int START_PANEL_ACTIVE = 0;
    /// <summary>
    /// 设置注册面板显示
    /// </summary>
    public const int REGIST_PANEL_ACTIVE = 1;


    /// <summary>
    /// 刷新信息面板
    /// </summary>
    public const int REFRESH_INFO_PANEL = 2;

    /// <summary>
    /// 显示进入房间的按钮
    /// </summary>
    public const int SHOW_ENTER_ROOM_BUTTON = 3;

    /// <summary>
    /// 设置创建面板
    /// </summary>
    public const int CREATE_PANEL_ACTIVE = 4;

    /// <summary>
    /// 设置底牌
    /// </summary>
    public const int SET_TABLE_CARDS = 5;

    /// <summary>
    /// 设置左面的角色的数据
    /// </summary>
    public const int SET_LEFT_PLAYER_DATA = 6;
    /// <summary>
    /// 角色准备
    /// </summary>
    public const int PLAYER_READY = 7;

    /// <summary>
    /// 角色进入
    /// </summary>
    public const int PLAYER_ENTER = 8;

    /// <summary>
    /// 角色离开
    /// </summary>
    public const int PLAYER_LEAVE = 9;

    /// <summary>
    /// 角色聊天
    /// </summary>
    public const int PLAYER_CHAT = 10;

    /// <summary>
    /// 角色更改身份
    /// </summary>
    public const int PLAYER_CHANGE_IDENTITY = 11;

    /// <summary>
    /// 开始游戏 角色隐藏状态面板
    /// </summary>
    public const int PLAYER_HIDE_SETATE = 12;

    /// <summary>
    /// 设置右面的角色的数据
    /// </summary>
    public const int SET_RIGHT_PLAYER_DATA = 13;


    /// <summary>
    /// 是否抢地主
    /// </summary>
    public const int SHOW_CRAB_BUTTON = 14;

    /// <summary>
    /// 出牌按钮
    /// </summary>
    public const int SHOW_DEA_BUTTON=15;

    /// <summary>
    /// 设置自身角色
    /// </summary>
    public const int SET_MY_PLAYER_DARA = 16;


    /// <summary>
    /// 玩家准备的按钮
    /// </summary>
    public const int PLAY_HIDE_READ_BUTTON = 17;


    /// <summary>
    /// 改变倍数
    /// </summary>
    public const int CHANGE_MUTIPLE = 18;

    /// <summary>
    /// 显示结束面板
    /// </summary>
    public const int SHOW_OVER_PANEL = 19;
 

    /// <summary>
    /// 发送log或提示消息
    /// </summary>
    public const int PROMPT_MSG = int.MaxValue;



}

