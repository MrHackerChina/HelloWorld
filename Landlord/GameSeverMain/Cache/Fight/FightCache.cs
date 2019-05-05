using GameSever.Util.Concurrent;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain.Cache.Fight
{
    /// <summary>
    /// 战斗的缓冲层
    /// </summary>
    public class FightCache
    {
        /// <summary>
        /// 用户id 对应的房间id
        /// </summary>
        private Dictionary<int, int> uidRoomDict = new Dictionary<int, int>();

        /// <summary>
        /// 房间的id 对应的 房间模型
        /// </summary>
        private Dictionary<int, FightRoom> idRoomDict = new Dictionary<int, FightRoom>();

        /// <summary>
        /// 重用的房间列表
        /// </summary>
        private Queue<FightRoom> roomQueu = new Queue<FightRoom>();

        /// <summary>
        /// 房间id
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 创建房间
        /// </summary>
        /// <returns></returns>
        public FightRoom Create(List<int> uidList)
        {
            FightRoom room = null;
            if (roomQueu.Count > 0)
            {
                room = roomQueu.Dequeue();

                room.Init(uidList);
            }
            else
                room = new FightRoom(id.Add_Get(), uidList);

            foreach (int uid in uidList)
            {
                uidRoomDict.Add(uid, room.Id);
            }
            idRoomDict.Add(room.Id, room);
            return room;
        }
        /// <summary>
        /// 获取房间模型  根据房间id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FightRoom GetRoom(int id)
        {
            if (idRoomDict.ContainsKey(id))
            {
                return idRoomDict[id];
            }
            else
            {
                throw new Exception("不存在这个房间");
            }
        }

        /// <summary>
        /// 是否在战斗房间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsFighting(int userId)
        {
            return uidRoomDict.ContainsKey(userId);
        }

        /// <summary>
        /// 获取房间模型  根据用户id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public FightRoom GetRoomByUId(int uid)
        {
            if (uidRoomDict.ContainsKey(uid))
            {
                return GetRoom(uidRoomDict[uid]);
            }
            else
            {
                throw new Exception("不存在这个房间");
            }
        }

        /// <summary>
        /// 摧毁房间
        /// </summary>
        /// <param name="room"></param>
        public void Destorry(FightRoom room)
        {
            idRoomDict.Remove(room.Id);
            foreach (PlayerDto player in room.PlayerList)
            {
                uidRoomDict.Remove(player.UserId);
            }
            room.PlayerList.Clear();
            room.LeaveUIdList.Clear();
            room.tableCardList.Clear();
            room.liabdrarModel.Init();
            room.Multiple = 1;
            room.roundModel.Init();
            roomQueu.Enqueue(room);
        }
    }
}
