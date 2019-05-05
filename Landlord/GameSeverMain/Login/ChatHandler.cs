using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSever;
using Protocol.Dto;
using GameSeverMain.Cache;
using GameSeverMain.Cache.Match;
using Protocol.Code;
using Protocol.Constant;

namespace GameSeverMain.Login
{
    class ChatHandler : IHandler
    {
        public UserCache userCache = Caches.User;
        private MatchCache matchCache = Caches.Match;
        public void OnDisconnent(ClientPeer client)
        {

        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case ChatCode.DEFAULT:
                    chatRequest(client, (int)value);
                break;
                default:
                    break;
            }
        }

        private void chatRequest(ClientPeer client,int chatType)
        {
            if (userCache.IsOnline(client) == false)
                return;
            int userId = userCache.Get_Id(client);
            ChatDto dto = new ChatDto(userId,chatType);
            if(matchCache.IsMatching(userId))
            {
                MatchRoom room = matchCache.GetRoom(userId);
                room.Brocast(OpCode.CHAT, ChatCode.SRES, dto);
            }
            else
            {
                //在这里检测战斗房间
                //TODO
            }
        }
    }
}
