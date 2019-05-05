using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class FightCode
    {
        #region   抢地主
       /// <summary>
        /// 抢地主
        /// </summary>
        public const int GRAB_LANDLORD_CREQ = 0;
        /// <summary>
        /// 广播抢地主
        /// </summary>
        public const int GRAB_LANDLORD_BRO = 1;

        /// <summary>
        /// 下一个玩家枪地主
        /// </summary>
        public const int TUR_GRAB_BRO = 2;
        #endregion

        #region  出牌

        /// <summary>
        /// 客户端发起出牌
        /// </summary>
        public const int DEAL_CREQ = 3;
        /// <summary>
        /// 单对客户端发起的响应
        /// </summary>
        public const int DEAL_SRES = 4;
        /// <summary>
        /// 服务器广播出牌
        /// </summary>
        public const int DEAL_BRO = 5;
        #endregion


        #region 不出牌
        /// <summary>
        /// 客户端发起不出牌
        /// </summary>
        public const int PASS_CREQ = 6;

        /// <summary>
        /// 服务器发客户端不出牌
        /// </summary>
        public const int PASS_SRES = 7;
        /// <summary>
        /// 服务器广播转换出牌
        /// </summary>
        public const int TURN_DEAL_BRO = 8;
        #endregion

        /// <summary>
        /// 有玩家离开
        /// </summary>
        public const int LEAVE_BRO = 9;

        /// <summary>
        /// 广播游戏结束
        /// </summary>
        public const int OVER_BRO = 10;

        /// <summary>
        /// 服务器给客户端卡牌的响应 发牌
        /// </summary>
        public const int GET_CADE_SERS = 11;

    }
}
