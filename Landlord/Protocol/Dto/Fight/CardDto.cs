
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 表示卡牌
    /// </summary>
    [Serializable]
    public class CardDto
    {
        public int Id;
        /// <summary>
        /// 路径
        /// </summary>
        public string Name;
        /// <summary>
        /// 花色
        /// </summary>
        public int Color;
        /// <summary>
        /// 数值
        /// </summary>
        public int Weight;


        public CardDto()
        {
        }
        public CardDto(string name,int color,int weight)
        {
            this.Name = name;
            this.Color = color;
            this.Weight = weight;
        }
    }
}
