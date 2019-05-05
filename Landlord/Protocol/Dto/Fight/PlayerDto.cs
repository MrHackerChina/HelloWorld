using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class PlayerDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId;

        /// <summary>
        /// 身份
        /// </summary>
        public int Identity;

        /// <summary>
        /// 自己拥有的手牌
        /// </summary>
        public List<CardDto> CardList;

        public PlayerDto(int userId)
        {
            this.UserId = userId;
            this.Identity = Protocol.Constant.Identity.FARMER;
            this.CardList = new List<CardDto>();
        }

        /// <summary>
        /// 是否还有手牌
        /// </summary>
        public bool HasCard
        {
            get{ return CardList.Count != 0; }
        }

        /// <summary>
        /// 添加手牌
        /// </summary>
        /// <param name="card"></param>
        public void Add(CardDto card)
        {
            CardList.Add(card);
        }

        /// <summary>
        /// 移除卡牌
        /// </summary>
        /// <param name="card"></param>
        public void Remove(CardDto card)
        {
            CardList.Remove(card);
        }

    }
}
