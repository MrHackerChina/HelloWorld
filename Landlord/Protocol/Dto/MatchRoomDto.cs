using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 房间数据对应的传输模型
    /// </summary>
    [Serializable]
    public class MatchRoomDto
    {
        /// <summary>
        /// 用户id对应的传输模型
        /// </summary>
        public Dictionary<int, UserDto> UIdUserDict;

        /// <summary>
        /// 准备的玩家id列表
        /// </summary>
        public List<int> ReadyUIdList;

        /// <summary>
        /// 储存玩家进入的顺序
        /// </summary>
        public List<int> UIdlist;

        public MatchRoomDto()
        {
            this.UIdUserDict = new Dictionary<int, UserDto>();
            this.ReadyUIdList = new List<int>();
            this.UIdlist = new List<int>();
        }

        public void Add(UserDto newUser)
        {
            this.UIdUserDict.Add(newUser.Id, newUser);
            this.UIdlist.Add(newUser.Id);
        }
        public void Leaver(int userId)
        {
            this.UIdUserDict.Remove(userId);
            this.UIdlist.Remove(userId);

        }
        public void Ready(int readyUser)
        {
            this.ReadyUIdList.Add(readyUser);
        }

        public int LeftId;
        public int RightId;
        /// <summary>
        /// 桌位的变化
        /// </summary>
        public void ResetPosition(int myUserId)
        {
            LeftId = -1;
            RightId = -1;
            int count = UIdlist.Count;
            if (count == 1)
                return;
            else if (count == 2)
            {
                if (UIdlist[0] == myUserId)
                    RightId = UIdlist[1];
                if (UIdlist[1] == myUserId)
                    LeftId = UIdlist[0];
            }
            else if (count == 3)
            {
                if (UIdlist[0] == myUserId)
                {
                    LeftId = UIdlist[2];
                    RightId = UIdlist[1];
                }
                if (UIdlist[1] == myUserId)
                {
                    LeftId = UIdlist[0];
                    RightId = UIdlist[2];
                }
                if (UIdlist[2] == myUserId)
                {
                    LeftId = UIdlist[1];
                    RightId = UIdlist[0];
                }

            }
        }
    }
}
