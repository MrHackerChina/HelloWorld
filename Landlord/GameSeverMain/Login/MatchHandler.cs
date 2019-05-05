using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSever;
using Protocol.Code;
using GameSeverMain.Cache.Match;
using GameSeverMain.Cache;
using Protocol.Dto;
using GameSeverMain.Model;

namespace GameSeverMain.Login
{
    public delegate void StartFight(List<int> uidList);

    /// <summary>
    /// 准备开始离开房间   85
    /// </summary>
    public class MatchHandler : IHandler
    {
        public StartFight startFight;

        private MatchCache matchCache = Caches.Match;
        private UserCache userCache = Caches.User;
       
        public void OnDisconnent(ClientPeer client)
        {
            if (!userCache.IsOnline(client))
                return;
            int userId = userCache.Get_Id(client);
            if (matchCache.IsMatching(userId))
            {
                leave(client);
            }
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case MatchCode.ENTER_CREQ:
                    enter(client);
                    break;
                case MatchCode.LEAVE_CREQ:
                    leave(client);
                    break;
                case MatchCode.READY_CREQ:
                    ready(client);
                    break;
                default:
                    break;
            }
        }
        #region 进入游戏
        private void enter(ClientPeer client)
        {
            SingleExecute.Instance.Excute(() =>
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.Get_Id(client);
                //用户是否在匹配
                if (matchCache.IsMatching(userId))
                {
                    //client.Send(OpCode.MATCH, MatchCode.EBTER_SERS, -1);
                    return;
                }
                MatchRoom room = matchCache.Enter(userId, client);
                //广播给房间内所有玩家
                UserModel model = userCache.GetModelById(userId);
                UserDto userDto = new UserDto(model.Id, model.Name, model.Been, model.WinCount, model.LoseCount, model.RunCount, model.Lv, model.Exp);
                room.Brocast(OpCode.MATCH, MatchCode.ENTER_BRO, userDto, client);
                //返回给当前的玩家
                MatchRoomDto dto = makeRoomDto(room);
                client.Send(OpCode.MATCH, MatchCode.ENTER_SERS, dto);
                Console.WriteLine("进入游戏");
            });
        }
        private MatchRoomDto makeRoomDto(MatchRoom room)
        {
            MatchRoomDto dto = new MatchRoomDto();
            foreach (var uid in room.UIdClientDict.Keys)
            {
                //if (userCache.IsOnline(uid) == false)
                //    continue;
                UserModel model = userCache.GetModelById(uid);
                UserDto userDto = new UserDto(model.Id,model.Name, model.Been, model.WinCount, model.LoseCount, model.RunCount, model.Lv, model.Exp);
                dto.UIdUserDict.Add(uid, userDto);
                dto.UIdlist.Add(uid);
            }
            dto.ReadyUIdList = room.ReadyUIdList;
            return dto;
        }
        #endregion

        #region 离开游戏
        private void leave(ClientPeer client)
        {
            SingleExecute.Instance.Excute(() =>
            {
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.Get_Id(client);
                if(matchCache.IsMatching(userId)==false)
                {
                    //client.Send(OpCode.MATCH,MatchCode.s)
                    return;
                }

                MatchRoom room = matchCache.Leave(userId);
                room.Brocast(OpCode.MATCH, MatchCode.LEAVE_BRO, userId);
                Console.WriteLine("离开游戏");

            });
        }

        #endregion

        #region  准备开始
        private void ready(ClientPeer client)
        {
            SingleExecute.Instance.Excute(() =>{
                if (!userCache.IsOnline(client))
                    return;
                int userId = userCache.Get_Id(client);
                if (matchCache.IsMatching(userId) == false)
                    return;
                MatchRoom room = matchCache.GetRoom(userId);
                room.Ready(userId);
                room.Brocast(OpCode.MATCH, MatchCode.READY_BRO, userId);
                //检测所有玩家
                if(room.IsAllReady())
                {
                    //TODO
                    // 通知客户端准备战斗
                    startFight(room.GetUIdList());


                    room.Brocast(OpCode.MATCH, MatchCode.START_BRO, null);
                    //销毁房间
                    matchCache.Destory(room);
                    Console.WriteLine("准备开始");
                }
            });
        }
        #endregion
    }
}

