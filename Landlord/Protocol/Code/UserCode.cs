
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    /// <summary>
    /// 用户的操作码  区分客户端和服务器
    /// </summary>
    public  class UserCode
   {
        /// <summary>
        /// 获取信息
        /// </summary>
        public const int GET_INFO_CREQ = 0;
        public const int GET_INFO_SRES = 1;

        /// <summary>
        /// 创建角色
        /// </summary>
        public const int CREATE_CREQ = 2;
        public const int CREATE_SERS = 3;

        /// <summary>
        /// 角色上线
        /// </summary>
        public const int ONLINE_CREQ = 3;
        public const int ONLINE_SERS = 4;
    }
}
