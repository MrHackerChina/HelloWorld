using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class MatchCode
    {

        /// <summary>
        /// 进入匹配
        /// </summary>
        public const int ENTER_CREQ = 0;
        public const int ENTER_SERS = 1;
        public const int ENTER_BRO = 2;

        /// <summary>
        /// 离开匹配
        /// </summary>
        public const int LEAVE_CREQ = 3;
        //public const int LEAVE_SERS = 3;
        public const int LEAVE_BRO = 4;
        /// <summary>
        /// 准备
        /// </summary>
        public const int READY_CREQ = 5;
        //public const int READY_SERS = 5;
        public const int READY_BRO = 6;

        /// <summary>
        /// 开始游戏
        /// </summary>
        //public const int START_CREQ = 6;
        //public const int START_SERS = 7;
        public const int START_BRO = 7;
    }
}
