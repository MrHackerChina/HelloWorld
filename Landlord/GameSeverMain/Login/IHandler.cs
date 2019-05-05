
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSever;

namespace GameSeverMain.Login
{
    public interface IHandler
    {
        /// <summary>
        /// 有消息发送
        /// </summary>
        /// <param name="client"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        void OnReceive(ClientPeer client, int subCode,object value);
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        void OnDisconnent(ClientPeer client);
    }
}
