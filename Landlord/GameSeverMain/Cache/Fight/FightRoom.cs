using GameSeverMain.Login;
using Protocol.Constant;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain.Cache.Fight
{
    /// <summary>
    /// 战斗房间
    /// </summary>
    public class FightRoom
    {
        /// <summary>
        /// 房间ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 储存所有的玩家
        /// </summary>
        public List<PlayerDto> PlayerList { get; set; }

        /// <summary>
        /// 中途退出的玩家id列表
        /// </summary>
        public List<int> LeaveUIdList { get; set; }

        /// <summary>
        /// 牌库
        /// </summary>
        public LiabraryModel liabdrarModel { get; set; }

        /// <summary>
        /// 底牌
        /// </summary>
        public List<CardDto> tableCardList { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int Multiple { get; set; }

        /// <summary>
        /// 回合管理
        /// </summary>
        public RoundModel roundModel { get; set; }

        private AIFightLogin aiFightLogin = null;

        public FightRoom(int id, List<int> uidList)
        {
            this.Id = id;
            this.PlayerList = new List<PlayerDto>();
            for (int i = 0; i < uidList.Count; i++)
            {
                PlayerDto player = new PlayerDto(uidList[i]);
                this.PlayerList.Add(player);
            }
            this.LeaveUIdList = new List<int>();
            this.liabdrarModel = new LiabraryModel();
            this.tableCardList = new List<CardDto>();
            this.Multiple = 1;
            this.roundModel = new RoundModel();
            this.aiFightLogin = new AIFightLogin();
        }

        /// <summary>
        /// 重用房间的初始化
        /// </summary>
        /// <param name="uidList"></param>
        public void Init(List<int> uidList)
        {
            for (int i = 0; i < uidList.Count; i++)
            {
                PlayerDto player = new PlayerDto(uidList[i]);
                this.PlayerList.Add(player);
            }
        }


        public bool IsOffline(int userUId)
        {
            return LeaveUIdList.Contains(userUId);
        }

        /// <summary>
        /// 转换出牌 下一个玩家
        /// </summary>
        /// <returns></returns>
        public int Turn()
        {
            int currId = roundModel.CurrentUId;
            int nextUid = GetNextUId(currId);
            roundModel.CurrentUId = nextUid;
            return nextUid;
        }
        public int GetNextUId(int currUId)
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].UserId == currUId)
                {
                    int nextUId = PlayerList[(i + 1) % 3].UserId;
                    return nextUId;
                }
            }
            throw new Exception("并没有这个出牌者");
        }


        /// <summary>
        /// 判断牌的大小
        /// </summary>
        /// <returns></returns>
        public bool DeadCard(int type, int weight, int length, int userId, List<CardDto> carList)
        {
            bool canDeal = false;
            if (type == roundModel.LastCardType && weight > roundModel.LastWeight)
            {
                if (type == CardType.STRAIGHT || type == CardType.DOUBLE_STRAIGHT || type == CardType.TRIPLE_STRAIGHT)
                {
                    if (length == roundModel.LastLength)
                    {
                        //满足条件
                        canDeal = true;
                    }
                }
                else
                {
                    canDeal = true;
                }
            }
            else if (type == CardType.BOOM && roundModel.LastCardType != CardType.BOOM)
            {
                canDeal = true;
            }
            else if (type == CardType.JOKER_BOOM)
            {
                canDeal = true;
            }
            else if (userId == roundModel.BiggestUId)
                canDeal = true;
            //出牌
            if (canDeal)
            {
                removeCards(userId, carList);
                if (type == CardType.BOOM)
                    Multiple *= 2;
                else if (type == CardType.JOKER_BOOM)
                    Multiple *= 4;
                //
                roundModel.Change(length, type, weight, userId);

            }

            return canDeal;
        }

        /// <summary>
        /// 获取玩家的现有手牌
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CardDto> GetUserCards(int userId)
        {
            foreach (PlayerDto play in PlayerList)
            {
                if (play.UserId == userId)
                    return play.CardList;
            }
            throw new Exception("不存在这个玩家");
        }

        /// <summary>
        /// 移除玩家的手牌
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardList"></param>
        public void removeCards(int userId, List<CardDto> cardList)
        {
            List<CardDto> currList = GetUserCards(userId);
            List<CardDto> list = new List<CardDto>();
            string severCardName = string.Empty;
            string nativeCardName = string.Empty;

            foreach (var select in cardList)
            {
                for (int i = currList.Count-1; i >= 0; i--)
                {
                    //判断牌的一致性
                    severCardName = CardColor.GetCardNamr(currList[i].Color, currList[i].Weight);
                    nativeCardName = CardColor.GetCardNamr(select.Color, select.Weight);
                    if(string.Equals(severCardName,nativeCardName))
                    {
                        list.Add(currList[i]);
                    }

                    //if (currList[i].Name==select.Name)
                    //{
                    //    list.Add(currList[i]);
                    //}
                }
            }
            foreach (var card in list)
                currList.Remove(card);
        }

        /// <summary>
        /// 发牌
        /// </summary>
        public void InitPlayerCards()
        {
            //按客户发
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 17; k++)
                {
                    CardDto card = liabdrarModel.Deal();
                    PlayerList[i].Add(card);
                }
            }
            //按顺序发牌
            //for (int i = 0; i < 17; i++)
            //{
            //    for (int k = 0; k < 3; k++)
            //    {
            //        CardDto card = liabdrarModel.Deal();
            //        PlayerList[k].Add(card);
            //    }
            //}

            for (int i = 0; i < 3; i++)
            {
                CardDto card = liabdrarModel.Deal();
                tableCardList.Add(card);
            }
        }

        /// <summary>
        /// 设置地主
        /// </summary>
        /// <param name="userId"></param>
        public void SetLandlord(int userId)
        {
            foreach (PlayerDto item in PlayerList)
            {
                if (item.UserId == userId)
                {
                    item.Identity = Identity.LANDLORD;
                    for (int i = 0; i < tableCardList.Count; i++)
                    {
                        item.Add(tableCardList[i]);
                    }
                    ///排序
                    Sort();
                    //开始回合
                    roundModel.Start(userId);
                    return;
                }
            }
        }

        /// <summary>
        /// 获取用户的数据模型
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PlayerDto GetPlayerModel(int userId)
        {
            foreach (PlayerDto player in PlayerList)
            {
                if (player.UserId == userId)
                {
                    return player;
                }
            }
            throw new Exception("没有这个玩家！！！");
        }

        /// <summary>
        /// 获取用户的身份
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetPlayerIdentity(int userId)
        {
            return GetPlayerModel(userId).Identity;
        }
        /// <summary>
        /// 或取相同身份的ID
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public List<int> GetSameIdentityUIds(int identity)
        {
            List<int> ids = new List<int>();
            foreach (PlayerDto player in PlayerList)
            {
                if (player.Identity == identity)
                {
                    ids.Add(player.UserId);
                }
            }
            return ids;
        }
        public List<int> GetDiffectIdentityUIds(int identity)
        {
            List<int> ids = new List<int>();
            foreach (PlayerDto player in PlayerList)
            {
                if (player.Identity != identity)
                {
                    ids.Add(player.UserId);
                }
            }
            return ids;
        }
        /// <summary>
        /// 获取房间内的第一个玩家id  开始当地主
        /// </summary>
        /// <returns></returns>
        public int GetFirstUId()
        {
            return PlayerList[0].UserId;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="cardDto"></param>
        private void sortCard(List<CardDto> cardList, bool asc = true)
        {
            cardList.Sort(
                (a, b) => {
                    if (asc)
                        return a.Weight.CompareTo(b.Weight);
                    else
                        return a.Weight.CompareTo(b.Weight) * -1;
                }
                );
        }

        /// <summary>
        /// 手牌排序
        /// </summary>
        /// <param name="asc"></param>
        public void Sort(bool asc = true)
        {
            for (int i = 0; i<PlayerList.Count; i++)
			{
                sortCard(PlayerList[i].CardList, asc);
			}
            sortCard(tableCardList, asc);
        }



        #region  AI 出牌 

        /// <summary>
        /// 出牌（自己不是最大）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="aiCardDto"></param>
        /// <returns></returns>
        public bool matchineCard(int userId ,ref AICardDto aiCardDto)
        {
            bool result = false;
            List<CardDto> cardDto = GetUserCards(userId);
            sortCard(cardDto);
            //自己最大
            if (roundModel.BiggestUId == userId)
            {
                aiCardDto.Change(true, cardDto[0].Weight, CardType.SINGE, new List<CardDto>{ cardDto[0] });
                roundModel.Change(1, CardType.SINGE, cardDto[0].Weight, userId);
                result = true;
            }
            else
            {
                if (cardDto.Count > 0)
                {
                    //TODO  排序卡牌
                    //sortCard(cardDto);
                    //没有结束  获取上次出牌的类型和权值 长度
                    int type = roundModel.LastCardType;
                    int weight = roundModel.LastWeight;
                    int length = roundModel.LastLength;
                    aiCardDto = aiFightLogin.isHaveCard(type, weight, length, cardDto);
                    if (aiCardDto.IsOut)
                        roundModel.Change(aiCardDto.CardDtoList.Count, aiCardDto.Type, aiCardDto.Weight, userId);
                    result = aiCardDto.IsOut;
                }
            }

            return result;
        }

        #endregion

    }

}
