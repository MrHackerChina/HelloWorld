using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
   public  class GrabDto
    {
        public int UserId;
        public List<CardDto> TableCardList;
        public List<CardDto> PlayerCardList;
        public GrabDto()
        {

        }

        public GrabDto(int id,List<CardDto> cards,List<CardDto> playerCardList)
        {
            this.UserId = id;
            this.TableCardList = cards;
            this.PlayerCardList = playerCardList;
        }
    }
}
