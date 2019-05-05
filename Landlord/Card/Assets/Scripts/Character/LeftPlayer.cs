using Protocol.Dto.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPlayer: OtherPlayerBase
{
    private void Awake()
    {
        Bind(CharacterEvent.INIT_LRFT_CARD,
            CharacterEvent.ADD_LEFT_CARS,
            CharacterEvent.REMOVE_LEFT_CARD);
    }
    public override void Exectue(int eventCode, object messagge)
    {
        switch (eventCode)
        {

            case CharacterEvent.INIT_LRFT_CARD:
                StartCoroutine(initCardList());
                break;
            case CharacterEvent.ADD_LEFT_CARS:
                addTableCard();
                break;
            case CharacterEvent.REMOVE_LEFT_CARD:
                removeCard((messagge as List<CardDto>).Count);
                break;
            default:
                break;
        }
    }


}
