using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CharacterEvent
{
    /// <summary>
    /// 初始化自身卡牌
    /// </summary>
    public const int INIT_MY_CARD = 0;
    /// <summary>
    ///  初始化左边卡牌
    /// </summary>
    public const int INIT_LRFT_CARD = 1;
    /// <summary>
    ///  初始化右边卡牌
    /// </summary>
    public const int INIT_RIGHT_CAED = 2;

    /// <summary>
    /// 给自身添加底牌
    /// </summary>
    public const int ADD_MY_CARS = 3;
    /// <summary>
    ///  给左边玩家添加底牌
    /// </summary>
    public const int ADD_LEFT_CARS = 4;
    /// <summary>
    ///  给右边玩家添加底牌
    /// </summary>
    public const int ADD_RIGHT_CARS = 5;

    /// <summary>
    /// 出牌
    /// </summary>
    public const int DEAL_CARD = 6;

    /// <summary>
    /// 移除自身手牌
    /// </summary>
    public const int REMOVE_MY_CARD = 7;
    /// <summary>
    /// 移除左手牌
    /// </summary>
    public const int REMOVE_LEFT_CARD = 8;
    /// <summary>
    /// 移除右手牌
    /// </summary>
    public const int REMOVE_RIGHT_CARD = 9;

    /// <summary>
    /// 更新桌面的显示
    /// </summary>
    public const int UPFATE_SHOW_DESK = 10;
}

