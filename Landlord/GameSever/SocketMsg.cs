using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSever
{
    /// <summary>
    /// 网络发送消息
    /// </summary>
    public class SocketMsg
    {
        /// <summary>
        /// 操作码
        /// </summary>
        public int OpCode { get; set; }
        /// <summary>
        /// 子操作
        /// </summary>
        public int SubCode { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public object Value { get; set; }

        public SocketMsg()
        {

        }

        public SocketMsg(int opCode,int subCode,object vaule)
        {
            this.OpCode = opCode;
            this.SubCode = subCode;
            this.Value = vaule;
        }
    }
}
