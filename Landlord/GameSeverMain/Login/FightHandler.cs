using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSever;
using GameSeverMain.Cache.Fight;
using GameSeverMain.Cache;
using Protocol.Code;
using Protocol.Dto.Fight;
using GameSeverMain.Model;

namespace GameSeverMain.Login
{
    public class FightHandler : IHandler
    {
        private FightCache fightCacha = Caches.Fight;
        private UserCache userCacha = Caches.User;
        public void OnDisconnent(ClientPeer client)
        {
            leave(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case FightCode.GRAB_LANDLORD_CREQ:
                    grablLandlord(client, (bool)value);
                    break;
                case FightCode.DEAL_CREQ:
                    deal(client, value as DealDto);
                    break;
                case FightCode.PASS_CREQ:
                    pass(client);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 抢地主
        /// </summary>
        /// <param name="client"></param>
        /// <param name="result"></param>

        private void grablLandlord(ClientPeer client,bool result)
        {
            SingleExecute.Instance.Excute(
                () =>
                {
                    if (userCacha.IsOnline(client) == false)
                        return;
                    int userId = userCacha.Get_Id(client);
                    FightRoom room = fightCacha.GetRoomByUId(userId);

                    if (result)
                    {
                        room.SetLandlord(userId);
                        //
                        GrabDto dto = new GrabDto(userId, room.tableCardList,room.GetUserCards(userId));
                        brocast(room, OpCode.FIGHT, FightCode.GRAB_LANDLORD_BRO, dto);
                    }
                    else
                    {
                        int nextUId = room.GetNextUId(userId);
                        brocast(room, OpCode.FIGHT, FightCode.TUR_GRAB_BRO, nextUId);
                    }

                }
                );
        }


        /// <summary>
        /// 出牌的处理
        /// </summary>
        private void deal(ClientPeer client,DealDto dto)
        {
            SingleExecute.Instance.Excute(
                ()=>
                {
                    if (!userCacha.IsOnline(client))
                        return;
                    int userId = userCacha.Get_Id(client);
                    if (userId != dto.UserId)
                        return;

                    FightRoom room = fightCacha.GetRoomByUId(userId);
                    if(room.LeaveUIdList.Contains(userId))
                    {
                        //直接转换出牌
                        turn(room);
                    }
                    bool canDeal = room.DeadCard(dto.Type, dto.Weight, dto.Length, userId, dto.SelectCardList);
                    if(canDeal)
                    {
                        client.Send(OpCode.FIGHT, FightCode.DEAL_SRES, 0);
                        //是否结束
                        List<CardDto> cards = room.GetPlayerModel(userId).CardList;
                        dto.RemaiCardList = cards;
                        //广播出牌
                        brocast(room, OpCode.FIGHT, FightCode.DEAL_BRO, dto);
                      
                        if(cards.Count==0)
                        {
                            //TODO gameover
                            gameOver(userId, room);
                        }
                        else
                        {
                            // 转换玩家
                            turn(room);
                        }
                    }
                    else
                    {
                        client.Send(OpCode.FIGHT, FightCode.DEAL_SRES, -1);
                    }
                }
                );
        }


        /// <summary>
        /// 不出牌的处理
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dto"></param>
        private void pass(ClientPeer client)
        {
            SingleExecute.Instance.Excute(
                () =>
                {
                    if (!userCacha.IsOnline(client))
                        return;
                    int userId = userCacha.Get_Id(client);
                    FightRoom room = fightCacha.GetRoomByUId(userId);
                    //最大的是自己
                    if(room.roundModel.BiggestUId==userId)
                    {
                        client.Send(OpCode.FIGHT, FightCode.PASS_SRES, -1);
                        return;
                    }
                    else
                    {
                        client.Send(OpCode.FIGHT, FightCode.PASS_SRES, 0);
                        turn(room);
                    }
                }
                );
        }

        /// <summary>
        /// 用户离开
        /// </summary>
        /// <param name="client"></param>
        private void leave(ClientPeer client)
        {
            SingleExecute.Instance.Excute(
                () =>
                {
                    if (!userCacha.IsOnline(client))
                        return;
                    int userId = userCacha.Get_Id(client);
                    //不在战斗的房间
                    if (!fightCacha.IsFighting(userId))
                        return;
                    FightRoom room = fightCacha.GetRoomByUId(userId);

                    //中途退出
                    room.LeaveUIdList.Add(userId);
                    brocast(room, OpCode.FIGHT, FightCode.LEAVE_BRO, userId);
                    //所有玩家都退出
                    if(room.LeaveUIdList.Count==3)
                    {
                        for (int i = 0; i < room.LeaveUIdList.Count; i++)
                        {
                            UserModel um = userCacha.GetModelById(room.LeaveUIdList[i]);
                            um.RunCount++;
                            um.Been -= room.Multiple * 1000 * 3;
                            //um.Exp += 0;
                            userCacha.Update(um);
                        }
                        fightCacha.Destorry(room);
                    }
                }
                );
        }


        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="room"></param>
        public void gameOver(int userId,FightRoom room)
        {
            int winIdentity = room.GetPlayerIdentity(userId);
            int winBeen = room.Multiple * 1000;
            List<int> winUIds = room.GetSameIdentityUIds(winIdentity);

            for (int i = 0; i < winUIds.Count; i++)
            {
                UserModel um = userCacha.GetModelById(winUIds[i]);
                um.WinCount++;
                um.Been += winBeen;
                um.Exp += 100;
                int maxExp = um.Lv * 100;
                while(maxExp<=um.Exp)
                {
                    um.Lv++;
                    um.Exp -= maxExp;
                    maxExp = um.Lv * 100;
                }
                userCacha.Update(um);
            }
            List<int> loseUIds = room.GetDiffectIdentityUIds(winIdentity);
            for (int i = 0; i < loseUIds.Count; i++)
            {
                UserModel um = userCacha.GetModelById(loseUIds[i]);
                um.LoseCount++;
                um.Been -= winBeen;
                um.Exp += 10;
                int maxExp = um.Lv * 100;
                while (maxExp <= um.Exp)
                {
                    um.Lv++;
                    um.Exp -= maxExp;
                    maxExp = um.Lv * 100;
                }
                userCacha.Update(um);
            }
            for (int i = 0; i < room.LeaveUIdList.Count; i++)
            {
                UserModel um = userCacha.GetModelById(room.LeaveUIdList[i]);
                um.RunCount++;
                um.Been -= winBeen * 3;
                //um.Exp += 0;
                int maxExp = um.Lv * 100;
                while (maxExp <= um.Exp)
                {
                    um.Lv++;
                    um.Exp -= maxExp;
                    maxExp = um.Lv * 100;
                }
                userCacha.Update(um);
            }
            // GameOver
            OverDto dto = new OverDto(winIdentity, winBeen, winUIds);
            brocast(room, OpCode.FIGHT, FightCode.OVER_BRO, dto);
            //摧毁房间
            fightCacha.Destorry(room);
        }


        /// <summary>
        /// 转换出牌
        /// </summary>
        /// <param name="room"></param>
        private void turn(FightRoom room)
        {
            int nextUId = room.Turn();
            if(room.IsOffline(nextUId))
            {
                //turn(room);
                Console.WriteLine(room.GetUserCards(nextUId).Count);
                AICardDto aiDto = new AICardDto();
                if(room.matchineCard(nextUId,ref aiDto))
                {
                    //可以出牌 获取出的牌
                    DealDto dto = new DealDto(aiDto.CardDtoList, nextUId);
                    //移除手牌
                    room.removeCards(nextUId, aiDto.CardDtoList);
                    //广播出牌
                    brocast(room, OpCode.FIGHT, FightCode.DEAL_BRO, dto);

                    if (room.GetUserCards(nextUId).Count == 0)
                    {
                        //TODO gameover
                        gameOver(nextUId, room);
                    }
                    else
                    {
                        // 转换玩家
                        turn(room);
                    }
                }
                else
                {
                    turn(room);
                }
            }
            else
            {
                ClientPeer nextClient = userCacha.GetClientPeer(nextUId);
                nextClient.Send(OpCode.FIGHT, FightCode.TURN_DEAL_BRO, nextUId);
            }
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="uidList"></param>
        public void startFight(List<int> uidList)
        {
            SingleExecute.Instance.Excute(
                () =>
                {
                    FightRoom room = fightCacha.Create(uidList);
                    room.InitPlayerCards();
                    room.Sort();
                    foreach (int  uid in uidList)
                    {
                        ClientPeer client = userCacha.GetClientPeer(uid);
                        List<CardDto> dto = room.GetUserCards(uid);
                        client.Send(OpCode.FIGHT, FightCode.GET_CADE_SERS, dto);
                    }
                    int firstUserID = room.GetFirstUId();
                    brocast(room, OpCode.FIGHT, FightCode.TUR_GRAB_BRO, firstUserID, null);
                }

            );
        }
        private void brocast (FightRoom room, int opCode, int subCode, object value, ClientPeer exClient = null)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMeg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);
            foreach (var player in room.PlayerList)
            {
                if (userCacha.IsOnline(player.UserId))
                {
                    ClientPeer client = userCacha.GetClientPeer(player.UserId);
                    if (client == exClient)
                        continue;
                    client.Send(packet);
                }
               
            }
        }

    }
}
