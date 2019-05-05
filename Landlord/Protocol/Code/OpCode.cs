using System;
using System.Collections.Generic;


namespace Protocol.Code
{
    public class OpCode
    {
        /// <summary> 
        /// 账号模块
        /// </summary>
        public const int ACCOUNT = 0;

        /// <summary>
        /// 用户模块
        /// </summary>
        public const int USER = 1;

        /// <summary>
        /// 匹配模块
        /// </summary>
        public const int MATCH = 2;

        /// <summary>
        /// 聊天模块
        /// </summary>
        public const int CHAT = 3;

        /// <summary>
        /// 战斗模块
        /// </summary>
        public const int FIGHT = 4;
    }
}
