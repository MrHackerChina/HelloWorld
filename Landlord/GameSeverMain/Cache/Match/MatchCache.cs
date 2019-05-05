using GameSever;
using GameSever.Util.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain.Cache.Match
{
    /// <summary>
    /// 匹配的缓冲层
    /// </summary>
    public class MatchCache
    {
        /// <summary>
        /// 正在等待的用户的id 和 房间的id
        /// </summary>
        private Dictionary<int, int> uidRoomIdDict = new Dictionary<int, int>();

        /// <summary>
        /// 等待的房间id  房间的数据模型
        /// </summary>
        private Dictionary<int, MatchRoom> idModelDict = new Dictionary<int, MatchRoom>();

        /// <summary>
        /// 房间的队列
        /// </summary>
        private Queue<MatchRoom> roomQueue = new Queue<MatchRoom>();

        private ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 进入匹配队列 进入匹配的房间
        /// </summary>
        /// <returns></returns>
        public MatchRoom Enter(int userId,ClientPeer client)
        {
            foreach (MatchRoom mr in idModelDict.Values)
            {
                if(mr.IsFull())
                    continue;
                mr.Enter(userId,client);
                uidRoomIdDict.Add(userId, mr.Id);
                return mr;
            }
            MatchRoom room = null;
            if (roomQueue.Count > 0)
                room = roomQueue.Dequeue();
            else
                room = new MatchRoom(id.Add_Get());
            room.Enter(userId, client);
            uidRoomIdDict.Add(userId, room.Id);
            idModelDict.Add(room.Id, room);
            return room;
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MatchRoom Leave(int userId)
        {
            int roomId = uidRoomIdDict[userId];
            MatchRoom room = idModelDict[roomId];
            room.Leave(userId);

            uidRoomIdDict.Remove(userId);
            if(room.IsEmpty())
            {
                idModelDict.Remove(roomId);
                roomQueue.Enqueue(room);
            }
            return room;
        }
      
        /// <summary>
        /// 用户是否在匹配
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsMatching (int userId)
        {
            return uidRoomIdDict.ContainsKey(userId);
        }

        /// <summary>
        /// 获取玩家等待的房间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MatchRoom GetRoom(int userId)
        {
            int roomId = uidRoomIdDict[userId];

            foreach (var item in idModelDict.Keys)
            {
                Console.WriteLine("房间号：" + item);
            }
            MatchRoom room = idModelDict[roomId];
            return room;
        }

        /// <summary>
        /// 摧毁房间
        /// </summary>
        public void Destory(MatchRoom room)
        {
            idModelDict.Remove(room.Id);
            foreach (var userId in room.UIdClientDict.Keys)
            {
                uidRoomIdDict.Remove(userId);
            }

            room.UIdClientDict.Clear();
            room.ReadyUIdList.Clear();
            roomQueue.Enqueue(room);
        }
     }
}
