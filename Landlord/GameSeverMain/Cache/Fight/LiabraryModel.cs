using Protocol.Constant;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain.Cache.Fight
{
    /// <summary>
    /// 牌库
    /// </summary>
    public class LiabraryModel
    {
        /// <summary>
        /// 所有牌的对列
        /// </summary>
        public Queue<CardDto> CardQueue { get; set;}

        public LiabraryModel()
        {
            create();
            shuffle();
        }

        public void Init()
        {
            create();
            shuffle();
        }
        /// <summary>
        /// 初始化牌
        /// </summary>
        private void create()
        {
            CardQueue = new Queue<CardDto>();
            //创建牌
            for (int color = CardColor.CLUB; color <= CardColor.SQUARE; color++)
            {
                for (int weight = CardWeight.THREE; weight <= CardWeight.TWO; weight++)
                {
                    //对应客户端的名称
                    CardDto card = new CardDto("", color, weight);
                    CardQueue.Enqueue(card);
                }
            }
            CardDto sJoker = new CardDto("", CardColor.NONE, CardWeight.SJOKER);
            CardDto iJoker = new CardDto("", CardColor.NONE, CardWeight.LJOKER);
            CardQueue.Enqueue(sJoker);
            CardQueue.Enqueue(iJoker);

        }


        /// <summary>
        /// 洗牌
        /// </summary>
        private void shuffle()
        {
            List<CardDto> tempList = new List<CardDto>();
            Random r = new Random();
            foreach (CardDto card in CardQueue)
            {
                int index = r.Next(0, tempList.Count + 1);
                tempList.Insert(index, card);
            }
            CardQueue.Clear();
            foreach (CardDto card in tempList)
            {
                CardQueue.Enqueue(card);
            }
        }
        
        /// <summary>
        /// 发牌
        /// </summary>
        /// <returns></returns>
        public CardDto Deal()
        {
            return CardQueue.Dequeue();
        }
    }
}
