using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPlayer : OtherPlayerBase {


    private void Awake()
    {
        Bind(CharacterEvent.INIT_RIGHT_CAED,
            CharacterEvent.ADD_RIGHT_CARS,
            CharacterEvent.REMOVE_RIGHT_CARD);
    }

    public override void Exectue(int eventCode, object messagge)
    {
        switch (eventCode)
        {
            case CharacterEvent.INIT_RIGHT_CAED:
                StartCoroutine(initCardList());
                break; ;
            case CharacterEvent.ADD_RIGHT_CARS:
                addTableCard();
                break;
            case CharacterEvent.REMOVE_RIGHT_CARD:
                removeCard((messagge as List<CardDto>).Count);
                break;
            default:
                break;
        }
    }
  
	
}
