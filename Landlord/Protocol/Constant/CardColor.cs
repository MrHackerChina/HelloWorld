using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
    public class CardColor
    {
        public const int NONE = 0;
        /// <summary>
        /// 梅花
        /// </summary>
        public const int CLUB = 1;
        /// <summary>
        /// 红桃
        /// </summary>
        public const int HEART = 2;
        /// <summary>
        /// 黑桃
        /// </summary>
        public const int SPADE = 3;
        /// <summary>
        /// 方片
        /// </summary>
        public const int SQUARE = 4;

        #region 获取本地对应的名称

        /// <summary>
        /// 获取完整的名称
        /// </summary>
        /// <param name="color"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static string GetCardNamr(int color,int weight)
        {
            return GetString(color) + GetWeight(weight);
        }

        /// <summary>
        /// 获取花色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetString(int color)
        {
            switch (color)
            {
                case CLUB:
                    return "Club";
                case HEART:
                    return "Heart";
                case SPADE:
                    return "Spade";
                case SQUARE:
                    return "Square";
                case NONE:
                    return "Jpker";
                default:
                    throw new Exception("不存在这样的花色");
            }
        }

        /// <summary>
        /// 获取权值
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static string GetWeight(int weight)
        {
            string result = string.Empty;
            if (weight <= 13)
                result = weight.ToString().PadLeft(2, '0');
            else if (weight <= 15)
                result = (weight - 13).ToString().PadLeft(2, '0');
            else 
                result= weight.ToString().PadLeft(2, '0');
            return result;

        }
        #endregion

    }
}
