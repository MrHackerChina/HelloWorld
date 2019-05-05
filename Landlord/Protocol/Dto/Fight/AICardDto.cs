using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// AI 出牌的模型
    /// </summary>
    public class AICardDto
    {

        /// <summary>
        /// 是否出牌
        /// </summary>
        public bool IsOut;

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight;

        /// <summary>
        /// 出牌的类型
        /// </summary>
        public int Type;


        /// <summary>
        /// 要出的牌
        /// </summary>
        public List<CardDto> CardDtoList;
        public AICardDto()
        {
            this.IsOut = false;
            this.Weight = -1;
            this.Type = -1;
            CardDtoList = new List<CardDto>();
        }
        /// <summary>
        /// 初始化 不能出牌
        /// </summary>
        /// <param name="isOut"></param>
        public AICardDto(bool isOut)
        {
            this.IsOut = isOut;
            this.Weight = -1;
            this.Type = -1;
            this.CardDtoList = null;
        }

        /// <summary>
        /// 初始化 可以出牌
        /// </summary>
        /// <param name="isOut"></param>
        /// <param name="weight"></param>
        /// <param name="type"></param>
        /// <param name="list"></param>
        public AICardDto(bool isOut,int weight,int type,List<CardDto> list)
        {
            this.IsOut = isOut;
            this.Weight = weight;
            this.Type = type;
            this.CardDtoList = list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isOut"></param>
        public void Change(bool isOut)
        {
            this.IsOut = isOut;
            this.Weight = -1;
            this.Type = -1;
            this.CardDtoList = null;
        }

        /// <summary>
        /// 改变 模型
        /// </summary>
        /// <param name="isOut"></param>
        /// <param name="weight"></param>
        /// <param name="type"></param>
        /// <param name="list"></param>
        public void Change(bool isOut, int weight, int type, List<CardDto> list)
        {
            this.IsOut = isOut;
            this.Weight = weight;
            this.Type = type;
            this.CardDtoList = list;
        }
    }
}
