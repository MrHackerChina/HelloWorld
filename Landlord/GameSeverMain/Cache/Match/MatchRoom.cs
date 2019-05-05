using GameSever;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain.Cache.Match
{
    /// <summary>
    /// 匹配的房间
    /// </summary>
    public class MatchRoom
    {
        private int RoomLimit = 3;
        /// <summary>
        /// 唯一的标识
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 在房间内用户ID列表  用户的连接对想
        /// </summary>
        public Dictionary<int, ClientPeer> UIdClientDict { get; private set; }

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetUIdList()
        {
            return UIdClientDict.Keys.ToList();
        }

        /// <summary>
        /// 已经准备的玩家ID列表
        /// </summary>
        public List<int> ReadyUIdList { get; private set; }

        public MatchRoom(int id)
        {
            this.Id = id;
            this.UIdClientDict = new Dictionary<int, ClientPeer>();
            this.ReadyUIdList = new List<int>();
        }

        /// <summary>
        /// 房间是否满了
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return UIdClientDict.Count >= RoomLimit;
        }
        /// <summary>
        /// room is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return UIdClientDict.Count == 0;
        }

        /// <summary>
        /// all user is ready
        /// </summary>
        /// <returns></returns>
        public bool IsAllReady()
        {
            return ReadyUIdList.Count == 3;
        }

        /// <summary>
        /// user is enter
        /// </summary>
        /// <param name="userId"></param>
        public void Enter(int userId, ClientPeer client)
        {
            UIdClientDict.Add(userId, client);
        }

        /// <summary>
        /// user is Leave
        /// </summary>
        /// <param name="userId"></param>
        public void Leave(int userId)
        {
            UIdClientDict.Remove(userId);
        }
        /// <summary>
        /// user is Ready
        /// </summary>
        /// <param name="userId"></param>
        public void Ready(int userId)
        {
            ReadyUIdList.Add(userId);
        }

        /// <summary>
        /// 广播房间内的玩家 除了自己
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        public void Brocast(int opCode, int subCode, object value, ClientPeer exClient = null)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMeg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);

            foreach (var client in UIdClientDict.Values)
            {
                if (client == exClient)
                    continue;
                client.Send(packet);
            }
        }
    }
}
