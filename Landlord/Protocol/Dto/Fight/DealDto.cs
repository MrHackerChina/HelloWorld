using Protocol.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class DealDto
    {
        /// <summary>
        /// 选中要出的牌
        /// </summary>
        public List<CardDto> SelectCardList;
        /// <summary>
        /// 长度
        /// </summary>
        public int Length;
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight;
        /// <summary>
        /// 类型
        /// </summary>
        public int Type;

        /// <summary>
        /// 谁出的牌
        /// </summary>
        public int UserId;

        /// <summary>
        /// 是否合法
        /// </summary>
        public bool IsRegular;

        public List<CardDto> RemaiCardList;

        public DealDto()
        {

        }
        public DealDto(List<CardDto> cardList,int uid)
        {
            this.SelectCardList = cardList;
            this.Length = cardList.Count;
            this.Type = CardType.GetCardType(cardList);
            this.Weight = CardWeight.GetWeight(cardList,this.Type);
            this.UserId = uid;
            this.IsRegular = (this.Type != CardType.NONE);
            this.RemaiCardList = new List<CardDto>();
        }

    }
}
