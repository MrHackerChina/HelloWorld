using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSever
{
    /// <summary>
    /// 客户端的连接池
    /// </summary>
    class ClientPeerPool
    {
        private Queue<ClientPeer> clientPeerQueue;

        public ClientPeerPool(int capacity)
        {
            clientPeerQueue = new Queue<ClientPeer>(capacity);
        }

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="client"></param>
        public void Enqueue(ClientPeer client)
        {
            clientPeerQueue.Enqueue(client);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public ClientPeer Depueue()
        {
            return clientPeerQueue.Dequeue();
        }
    }
}
