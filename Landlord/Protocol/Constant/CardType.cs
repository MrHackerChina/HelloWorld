using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{

    /// <summary>
    /// 出牌的类型
    /// </summary>
    public class CardType
    {
        /// <summary>
        /// 
        /// </summary>
        public const int NONE = 0;
        /// <summary>
        /// 单牌
        /// </summary>
        public const int SINGE = 1;
        /// <summary>
        /// 对
        /// </summary>
        public const int DOUBLE = 2;
        /// <summary>
        /// 顺子
        /// </summary>
        public const int STRAIGHT = 3;
        /// <summary>
        /// 双顺
        /// </summary>
        public const int DOUBLE_STRAIGHT = 4;
        /// <summary>
        /// 3顺
        /// </summary>
        public const int TRIPLE_STRAIGHT = 5;
        /// <summary>
        /// 3不带
        /// </summary>
        public const int THREE = 6;
        /// <summary>
        /// 3带1
        /// </summary>
        public const int THREE_ONE = 7;
        /// <summary>
        /// 3带2
        /// </summary>
        public const int THREE_TWO = 8;
        /// <summary>
        /// 炸弹
        /// </summary>
        public const int BOOM = 9;
        /// <summary>
        /// 王炸
        /// </summary>
        public const int JOKER_BOOM = 10;



        #region 选中牌后的逻辑判断
        /// <summary>
        /// 是否是单牌
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsSingle(List<CardDto> cards)
        {
            return cards.Count == 1;
        }

        /// <summary>
        /// 是否是对
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDouble(List<CardDto> cards)
        {

            if (cards.Count == 2)
            {
                if (cards[0].Weight == cards[1].Weight)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是不是顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsStraight(List<CardDto> cards)
        {
            if (cards.Count < 5 || cards.Count > 12)
                return false;

            for (int i = 0; i < cards.Count - 1; i++)
            {
                int tempWeight = cards[i].Weight;
                if (cards[i + 1].Weight - tempWeight != 1)
                    return false;
                if (tempWeight > CardWeight.ONE || cards[i + 1].Weight > CardWeight.ONE)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 是否是双顺
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDoubleStraight(List<CardDto> cards)
        {
            if (cards.Count < 6 || cards.Count % 2 != 0)
                return false;

            for (int i = 0; i < cards.Count - 2; i += 2)
            {
                if (cards[i].Weight != cards[i + 1].Weight)
                    return false;
                if (cards[i + 2].Weight - cards[i].Weight != 1)
                    return false;
                if (cards[i].Weight > CardWeight.ONE || cards[i + 2].Weight > CardWeight.ONE)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 是否是飞机
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsTripleStraight(List<CardDto> cards)
        {
            if (cards.Count < 6 || cards.Count % 3 != 0)
                return false;
            for (int i = 0; i < cards.Count - 3; i += 3)
            {
                if (cards[i].Weight != cards[i + 1].Weight)
                    return false;
                if (cards[i].Weight != cards[i + 2].Weight)
                    return false;
                if (cards[i + 1].Weight != cards[i + 2].Weight)
                    return false;

                if (cards[i + 3].Weight - cards[i].Weight != 1)
                    return false;
                if (cards[i].Weight > CardWeight.ONE || cards[i + 3].Weight > CardWeight.ONE)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 3不带
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThree(List<CardDto> cards)
        {
            if (cards.Count != 3)
                return false;
            if (cards[0].Weight != cards[1].Weight)
                return false;
            if (cards[0].Weight != cards[2].Weight)
                return false;
            if (cards[1].Weight != cards[2].Weight)
                return false;
            return true;
        }
        /// <summary>
        /// 3带1
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndOne(List<CardDto> cards)
        {
            if (cards.Count != 4)
                return false;
            if (cards[0].Weight == cards[1].Weight && cards[1].Weight == cards[2].Weight)
                return true;
            if (cards[1].Weight == cards[2].Weight && cards[2].Weight == cards[3].Weight)
                return true;
            return false;
        }
        /// <summary>
        /// 3带2
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndTwo(List<CardDto> cards)
        {

            if (cards.Count != 5)
                return false;
            if (cards[0].Weight == cards[1].Weight && cards[1].Weight == cards[2].Weight)
            {
                if (cards[3].Weight == cards[4].Weight)
                    return true;
            }
            else if (cards[2].Weight == cards[3].Weight && cards[3].Weight == cards[4].Weight)
            {
                if (cards[0].Weight == cards[1].Weight)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是不是炸弹
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsBoom(List<CardDto> cards)
        {
            if (cards.Count != 4)
                return false;
            if (cards[0].Weight != cards[1].Weight)
                return false;
            if (cards[1].Weight != cards[2].Weight)
                return false;
            if (cards[2].Weight != cards[3].Weight)
                return false;
            return true;
        }
        /// <summary>
        /// 是不是王炸
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsJokerBoom(List<CardDto> cards)
        {
            if (cards.Count != 2)
                return false;
            if (cards[0].Weight == CardWeight.LJOKER && cards[1].Weight == CardWeight.SJOKER)
                return true;
            else if (cards[1].Weight == CardWeight.LJOKER && cards[0].Weight == CardWeight.SJOKER)
                return true;
            return false;
        }

        public static int GetCardType(List<CardDto> cardsList)
        {
            int cardType = CardType.NONE;
            switch (cardsList.Count)
            {
                case 1:
                    if (IsSingle(cardsList))
                        cardType = CardType.SINGE;
                    break;
                case 2:
                    if (IsDouble(cardsList))
                        cardType = CardType.DOUBLE;
                    else if (IsDouble(cardsList))
                        cardType = CardType.JOKER_BOOM;
                    break;
                case 3:
                    if (IsThree(cardsList))
                        cardType = CardType.THREE;
                    break;
                case 4:
                    if (IsBoom(cardsList))
                        cardType = CardType.BOOM;
                    else if (IsThreeAndOne(cardsList))
                        cardType = CardType.THREE_ONE;
                    break;
                case 5:
                    if (IsStraight(cardsList))
                        cardType = CardType.STRAIGHT;
                    else if (IsThreeAndTwo(cardsList))
                        cardType = CardType.THREE_TWO;
                    break;
                case 6:
                    if (IsStraight(cardsList))
                        cardType = CardType.STRAIGHT;
                    else if (IsDoubleStraight(cardsList))
                        cardType = CardType.DOUBLE_STRAIGHT;
                    else if (IsTripleStraight(cardsList))
                        cardType = CardType.TRIPLE_STRAIGHT;
                    break;
                case 7:
                    if (IsStraight(cardsList))
                        cardType = CardType.STRAIGHT;
                    break;
                case 8:
                    if (IsStraight(cardsList))
                        cardType = CardType.STRAIGHT;
                    else if (IsDoubleStraight(cardsList))
                        cardType = CardType.DOUBLE_STRAIGHT;
                    break;
                case 9:
                    if (IsStraight(cardsList))
                        cardType = CardType.STRAIGHT;
                    else if (IsTripleStraight(cardsList))
                        cardType = CardType.TRIPLE_STRAIGHT;
                    break;
                case 10:
                    if (IsStraight(cardsList))
                        cardType = CardType.STRAIGHT;
                    else if (IsDoubleStraight(cardsList))
                        cardType = CardType.DOUBLE_STRAIGHT;
                    break;
                case 11:
                    if (IsStraight(cardsList))
                        cardType = CardType.STRAIGHT;
                    break;
                case 12:
                    if (IsStraight(cardsList))
                        cardType = CardType.STRAIGHT;
                    else if (IsDoubleStraight(cardsList))
                        cardType = CardType.DOUBLE_STRAIGHT;
                    else if (IsTripleStraight(cardsList))
                        cardType = CardType.TRIPLE_STRAIGHT;
                    break;
                case 13:
                    break;
                case 14:
                    if (IsDoubleStraight(cardsList))
                        cardType = CardType.DOUBLE_STRAIGHT;
                    break;
                case 15:
                    if (IsTripleStraight(cardsList))
                        cardType = CardType.TRIPLE_STRAIGHT;
                    break;
                case 16:
                    if (IsDoubleStraight(cardsList))
                        cardType = CardType.DOUBLE_STRAIGHT;
                    break;
                case 17:
                    break;
                case 18:
                    if (IsDoubleStraight(cardsList))
                        cardType = CardType.DOUBLE_STRAIGHT;
                    else if (IsTripleStraight(cardsList))
                        cardType = CardType.TRIPLE_STRAIGHT;
                    break;
                case 19:
                    break;
                case 20:
                    if (IsDoubleStraight(cardsList))
                        cardType = CardType.DOUBLE_STRAIGHT;
                    break;
                default:
                    break;
            }
            return cardType;
        }
        #endregion


        #region AI选牌中的处理


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="weight"></param>
        /// <param name="lenght"></param>
        /// <param name="cardList"></param>
        public static AICardDto isHaveCard(int type, int weight, int lenght, List<CardDto> cardList)
        {
            AICardDto aiCardDto = new AICardDto();
            switch (type)
            {
                case SINGE:
                    // 是单牌
                    aiCardDto = getSinge(weight, cardList);
                    break;
                case DOUBLE:
                    aiCardDto = getDouble(weight, cardList);
                    break;
                case STRAIGHT:
                    aiCardDto = getStraight(type, weight, lenght, cardList);
                    break;
                case DOUBLE_STRAIGHT:
                    aiCardDto = isDouble_Straight(type, weight, lenght, cardList);
                    break;
                case TRIPLE_STRAIGHT:
                    aiCardDto = isTriple_Straight(type, weight, lenght, cardList);
                    break;
                case THREE:
                    aiCardDto = getThree(weight,type, cardList);
                    break;
                case THREE_ONE:
                    aiCardDto = getThree_Base(weight, type, 1, cardList);
                    break;
                case THREE_TWO:
                    aiCardDto = getThree_Base(weight, type, 2, cardList);
                    break;
                case BOOM:
                    aiCardDto = get_Boom(weight, cardList);
                    break;
                case JOKER_BOOM:
                    break;
                default:
                    break;
            };
            return aiCardDto;
        }

        #region 单牌的处理
        /// <summary>
        /// 单牌大于
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static AICardDto getSinge(int weight, List<CardDto> cards)
        {
            List<CardDto> result = new List<CardDto>();
            int cardType = NONE;
            int cardWeight = 0;
            //单个牌大于
            for (int i = 0; i < cards.Count; i++)
            {
                //权值大于
                if (cards[i].Weight > weight)
                {
                    int count = 0;
                    for (int k = 0; k < cards.Count; k++)
                    {
                        if (cards[k].Weight == cards[i].Weight)
                            count++;
                    }
                    if (count == 4) //是炸弹 不能出
                        continue;
                    else
                    {
                        result.Add(cards[i]);
                        cardWeight = cards[i].Weight;
                        cardType = SINGE;
                        break;
                    }
                }
            }
            //是否可以用炸弹
            if (result.Count == 0)
            {
                Dictionary<int, List<CardDto>> dic = combinationCards(cards);
                bool isBoom = false;
                int temp = 0;
                foreach (int item in dic.Keys)
                {
                    //是4张牌的炸弹
                    if (dic[item].Count == 4)
                    {
                        isBoom = true;
                        temp = item;
                        break;
                    }
                }
                if (isBoom)
                {
                    result = dic[temp];
                    cardWeight = result[0].Weight * 4;
                    cardType = BOOM;
                }
                else // 看是否有王炸
                {
                    if (dic.ContainsKey(CardWeight.SJOKER) && dic.ContainsKey(CardWeight.LJOKER))
                    {
                        //获取大小王
                        result.AddRange(dic[CardWeight.SJOKER]);
                        result.AddRange(dic[CardWeight.LJOKER]);

                        cardWeight = CardWeight.SJOKER + CardWeight.LJOKER;
                        cardType = JOKER_BOOM;
                    }

                }
            }
            bool isOut = result.Count > 0 ? true : false;

            AICardDto dto = new AICardDto();
            if (isOut)
                dto.Change(isOut, cardWeight, cardType, result);
            else
                dto.Change(isOut);
            return dto;
        }

        /// <summary>
        /// 组合排牌的个数  权重 --  牌的模型
        /// </summary>
        /// <param name="cardList"></param>
        /// <returns></returns>
        private static Dictionary<int, List<CardDto>> combinationCards(List<CardDto> cardList)
        {
            Dictionary<int, List<CardDto>> result = new Dictionary<int, List<CardDto>>();

            int weight = 0;
            for (int i = 0; i < cardList.Count; i++)
            {
                weight = cardList[i].Weight;
                if (!result.ContainsKey(weight))
                    result[weight] = new List<CardDto>();
                result[weight].Add(cardList[i]);
            }
            return result;
        }
        #endregion


        #region 是否有炸弹



        /// <summary>
        /// 准备出炸弹
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static AICardDto get_Boom(int weight, List<CardDto> cards)
        {
            AICardDto aiCardDto = new AICardDto();
            if (isBoom(cards, weight, ref aiCardDto))
                return aiCardDto;
            if (isJoker_Boom(cards, ref aiCardDto))
                return aiCardDto;
            return aiCardDto;
        }

        /// <summary>
        /// 是否有王炸
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="aiCardDto"></param>
        /// <returns></returns>
        private static bool isJoker_Boom(List<CardDto> cards, ref AICardDto aiCardDto)
        {
            Dictionary<int, List<CardDto>> dic = combinationCards(cards);
            bool result = false;
            if (dic.ContainsKey(CardWeight.SJOKER) && dic.ContainsKey(CardWeight.LJOKER))
            {
                List<CardDto> list = new List<CardDto>();
                list.AddRange(dic[CardWeight.SJOKER]);
                list.AddRange(dic[CardWeight.LJOKER]);
                aiCardDto.Change(true, CardWeight.SJOKER + CardWeight.LJOKER, CardType.JOKER_BOOM, list);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 是否有常规炸弹
        /// </summary>
        /// <param name="cards">手牌</param>
        /// <param name="weight">单牌的权重 </param>
        /// <param name="aiCardDto"></param>
        /// <returns></returns>
        private static bool isBoom(List<CardDto> cards, int weight, ref AICardDto aiCardDto)
        {
            Dictionary<int, List<CardDto>> dic = combinationCards(cards);
            bool result = false;
            foreach (int item in dic.Keys)
            {
                //有炸弹 并且大于给定的炸弹
                if (dic[item].Count == 4 && dic[item][0].Weight > weight)
                {
                    aiCardDto.Change(true, dic[item][0].Weight * 4, CardType.BOOM, dic[item]);
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion

        #region 对的处理
        private static AICardDto getDouble(int weight, List<CardDto> cards)
        {
            AICardDto aiCardDto = new AICardDto();
            //牌数不够
            if (cards.Count <= 1)
            {
                aiCardDto.Change(false, -1, -1, null);
                return aiCardDto;
            }
            Dictionary<int, List<CardDto>> dic = combinationCards(cards);
            foreach (int item in dic.Keys)
            {
                if (item * 2 > weight)
                {
                    // 是对但并不能为炸弹
                    if (dic[item].Count >= 2 && dic[item].Count != 4)
                    {
                        aiCardDto.Change(true, item * 2, CardType.DOUBLE, new List<CardDto> { dic[item][0], dic[item][1] });
                        break;
                    }
                }
            }
            //没有对 
            if (!aiCardDto.IsOut)
            {
                foreach (int item in dic.Keys)
                {
                    //有炸弹
                    if (dic[item].Count == 4)
                    {
                        aiCardDto.Change(true, item * 4, CardType.BOOM, dic[item]);
                        break;
                    }
                }
            }
            //没有常规的炸弹 
            if (!aiCardDto.IsOut)
            {
                isJoker_Boom(cards, ref aiCardDto);
            }
            return aiCardDto;
        }

        #endregion

        #region 判断手牌中可以有顺子
        private static AICardDto getStraight(int type, int weight, int lenght, List<CardDto> cards)
        {
            AICardDto aiCardDto = new AICardDto();
            isStraight(type, weight, lenght, 1, cards, ref aiCardDto);
            if (!aiCardDto.IsOut)
            {
                if (isBoom(cards, 0, ref aiCardDto))
                    return aiCardDto;
                if (isJoker_Boom(cards, ref aiCardDto))
                    return aiCardDto;
            }
            return aiCardDto;
        }

        private static bool isStraight(int type, int weight, int lenght, int count, List<CardDto> cards, ref AICardDto aiCardDto)
        {
            Dictionary<int, List<CardDto>> dic = combinationCards(cards);
            bool result = false;
            //没有合适的顺子
            if (dic.Count >= lenght)
            {
                List<int> keys = new List<int>(dic.Keys);
                int nextLenght = 0;
                int index = 0;
                // 判断 长度
                for (int i = 0; i < keys.Count; i++)
                {
                    //下一个索引的位置的权值(判断长度)
                    nextLenght = keys[i] + (lenght/count) - 1;
                    if (nextLenght > CardWeight.ONE)
                        continue;
                    index = i + (lenght/count) - 1;
                    if (keys.Count > index)
                    {
                        //判断权重值相等
                        if (keys[index] == nextLenght)
                        {
                            int values = 0;
                            //计算权重的和
                            for (int k = i; k <= index; k++)
                            {
                                values += keys[k];
                            }
                            if (values > weight/count)
                            {
                                List<CardDto> list = new List<CardDto>();
                                bool isLegal = true;
                                for (int k = i; k <= index; k++)
                                {
                                    if (dic[keys[k]].Count >= count)
                                    {
                                        for (int j = 0; j < count; j++)
                                            list.Add(dic[keys[k]][j]);
                                    }
                                    else
                                    {
                                        //不合格
                                        isLegal = false;
                                        break;
                                    }
                                }
                                if(isLegal)
                                {
                                    aiCardDto.Change(true, values * count, type, list);
                                    result = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region 是否是双顺（334455...)

        /// <summary>
        /// 判断是不是双顺
        /// </summary>
        /// <param name="type"></param>
        /// <param name="weight"></param>
        /// <param name="lenght"></param>
        /// <param name="count"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static  AICardDto isDouble_Straight(int type, int weight, int lenght, List<CardDto> cards)
        {
            return is_Straight(type, weight, lenght, 2, cards);
        }

        /// <summary>
        ///  判断是不是顺子的格式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="weight"></param>
        /// <param name="lenght"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static AICardDto is_Straight(int type,int weight, int lenght, int count, List<CardDto> cards)
        {
            AICardDto aiCardDto = new AICardDto();
            isStraight(type, weight, lenght, count, cards, ref aiCardDto);
            if (!aiCardDto.IsOut)
            {
                if (isBoom(cards, 0, ref aiCardDto))
                    return aiCardDto;
                if (isJoker_Boom(cards, ref aiCardDto))
                    return aiCardDto;
            }
            return aiCardDto;

        }
        #endregion


        #region 是否是三顺（333444555...）

        /// <summary>
        /// 是否是3顺
        /// </summary>
        /// <param name="type"></param>
        /// <param name="weight"></param>
        /// <param name="lenght"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static AICardDto isTriple_Straight(int type, int weight, int lenght, List<CardDto> cards)
        {
            return is_Straight(type, weight, lenght, 3, cards);
        }
        #endregion

        #region  是否是三带
        /// <summary>
        /// 获取三不带
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="type"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static AICardDto getThree(int weight,int type, List<CardDto> cards)
        {
            AICardDto aiCardDto = new AICardDto();
            aiCardDto = getThree_Card(weight,type, cards);
            if (!aiCardDto.IsOut)
            {
                aiCardDto = get_Boom(0, cards);
            }
            return aiCardDto;
        }

        /// <summary>
        /// 获取3带 
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="type"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static AICardDto getThree_Card(int weight,int type,List<CardDto> cards)
        {
            AICardDto aiCardDto = new AICardDto();
            Dictionary<int, List<CardDto>> dic = combinationCards(cards);
            List<int> list = new List<int>(dic.Keys);
            int temp = 0;
            for (int i = 0; i < list.Count; i++)
            {
                temp = list[i];
                //判断有几个牌  并且权值要大于
                if (dic[temp].Count >= 3 && temp > weight / 3)
                {
                    List<CardDto> dto = new List<CardDto>();
                    for (int k = 0; k < 3; k++)
                        dto.Add(dic[temp][k]);
                    aiCardDto.Change(true, temp * 3, type, dto);
                    break;
                }
            }
            return aiCardDto;
        }

        #endregion


        #region 是否是三带一
        private static bool getThree_ElseCard(int weight,int count,List<CardDto> cards,ref List<CardDto> refList)
        {
            bool result = false;
            Dictionary<int, List<CardDto>> dic = combinationCards(cards);
            List<int> list = new List<int>(dic.Keys);
            int temp = 0;
            
            for (int i = 0; i < list.Count; i++)
            {
                temp = list[i];
                if (temp != weight && dic[temp].Count >= count)
                {
                    result = true;
                    for (int k = 0; k < count; k++)
                        refList.Add(dic[temp][k]);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        ///  获取三带一
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        private static AICardDto getThree_Base(int weight,int type,int count, List<CardDto> cards)
        {
            AICardDto aiCardDto = new AICardDto();
            aiCardDto = getThree_Card(weight, type,cards);
            if(aiCardDto.IsOut)
            {
                List<CardDto> list = new List<CardDto>();
                if (getThree_ElseCard(weight, count, cards, ref list))
                {
                    //有其他牌
                    aiCardDto.CardDtoList.AddRange(list);
                }
                else
                {
                    // 没有其他牌
                    aiCardDto.Change(false);
                }
            }

            if(!aiCardDto.IsOut)
            {
                aiCardDto = get_Boom(0, cards);
            }
            return aiCardDto;
        }

        #endregion


        #endregion

    }
}
