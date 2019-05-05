using Protocol.Code;
using Protocol.Dto;
using Protocol.Dto.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol.Constant;

public class FightHandler : HanderBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case FightCode.GET_CADE_SERS:
                getCards(value as List<CardDto>);
                break;
            case FightCode.TUR_GRAB_BRO:
                turnGrabBro((int)value);
                break;
            case FightCode.GRAB_LANDLORD_BRO:
                grabLandLord(value as GrabDto);
                break;
            case FightCode.DEAL_BRO:
                dealBro(value as DealDto);
                break;
            case FightCode.DEAL_SRES:
                dealResponse((int)value);
                break;
            case FightCode.OVER_BRO:
                overBro(value as OverDto);
                break;
            case FightCode.TURN_DEAL_BRO:
                turnBro((int)value);
                break;
            case FightCode.PASS_SRES:
                passResponse((int)value);
                break; ;
               
            default:
                break;
        }
    }
    private PromptMsg promptMsg = new PromptMsg();
   
    /// <summary>
    /// 获得卡牌  相当于发牌
    /// </summary>
    /// <param name="cardList"></param>
    private void getCards(List<CardDto> cardList)
    {
        //发牌
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_MY_CARD, cardList);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_LRFT_CARD, null);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.INIT_RIGHT_CAED, null);

        //改变倍数
        Dispatch(AreaCode.UI, UIEvent.CHANGE_MUTIPLE, 1);
    }

    private bool IsFirst = true;
    /// <summary>
    /// 是否抢地主
    /// </summary>
    /// <param name="userId"></param>
    private void turnGrabBro(int userId)
    {
        if(IsFirst)
        {
            IsFirst = false;
        }
        else
        {
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/Woman_NoOrder");
        }
        
        if (userId == Models.GameModel.UserDto.Id)
            Dispatch(AreaCode.UI, UIEvent.SHOW_CRAB_BUTTON, true);
    }

    /// <summary>
    /// 抢地主成功
    /// </summary>
    /// <param name="dto"></param>
    private void grabLandLord(GrabDto dto)
    {
        Dispatch(AreaCode.UI, UIEvent.PLAYER_CHANGE_IDENTITY, dto.UserId);

        Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "Fight/Woman_Order");

        //显示底牌
        Dispatch(AreaCode.UI, UIEvent.SET_TABLE_CARDS, dto.TableCardList);

        //发地主牌
        int userId = dto.UserId;
        int eventCode = -1;
        MatchRoomDto roomDto = Models.GameModel.MatchRoomDto;

        if (userId == roomDto.LeftId)
            eventCode = CharacterEvent.ADD_LEFT_CARS;
        else if (userId == roomDto.RightId)
            eventCode = CharacterEvent.ADD_RIGHT_CARS;
        else if (userId == Models.GameModel.UserDto.Id)
        {
            eventCode = CharacterEvent.ADD_MY_CARS;
            Dispatch(AreaCode.UI, UIEvent.SHOW_DEA_BUTTON, true);
        }
        Dispatch(AreaCode.CHARACTER, eventCode, dto);
        //显示出牌按钮
    }

    /// <summary>
    /// 同步出牌
    /// </summary>
    /// <param name="dto"></param>
    private void dealBro(DealDto dto)
    {
        //移除出牌的手牌
        int eventCode = -1;
        if (dto.UserId == Models.GameModel.MatchRoomDto.LeftId)
            eventCode = CharacterEvent.REMOVE_LEFT_CARD;
        else if (dto.UserId == Models.GameModel.MatchRoomDto.RightId)
            eventCode = CharacterEvent.REMOVE_RIGHT_CARD;
        else if (dto.UserId == Models.GameModel.UserDto.Id)
            eventCode = CharacterEvent.REMOVE_MY_CARD;
        Dispatch(AreaCode.CHARACTER, eventCode, dto.RemaiCardList);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.UPFATE_SHOW_DESK, dto.SelectCardList);

        playDealAudio(dto.Type, dto.Weight);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="cardType"></param>
    /// <param name="weight"></param>
    private void playDealAudio(int cardType, int weight)
    {
        string audioName = "Fight/";
        switch (cardType)
        {
            case CardType.SINGE:
                audioName += "";
                break;
            case CardType.DOUBLE:
                audioName += "";
                break;
            case CardType.STRAIGHT:
                audioName += "";
                break;
            case CardType.DOUBLE_STRAIGHT:
                audioName += "";
                break;
            case CardType.TRIPLE_STRAIGHT:
                audioName += "";
                break;
            case CardType.THREE:
                audioName += "";
                break;
            case CardType.THREE_ONE:
                audioName += "";
                break;
            case CardType.THREE_TWO:
                audioName += "";
                break;
            case CardType.BOOM:
                audioName += "";
                break;
            case CardType.JOKER_BOOM:
                audioName += "";
                break;
            default:
                break;
        }
        Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, audioName);
    }


    /// <summary>
    /// 出牌响应
    /// </summary>
    /// <param name="result"></param>
    private void dealResponse(int result)
    {
        bool isActive = false;
        if (result == -1)
        {
            promptMsg.Change("管不上", Color.red);
            //Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            //显示出牌
            isActive = true;
        }
        else if (result == 0)
            promptMsg.Change("出牌成功...", Color.red);

        Dispatch(AreaCode.UI, UIEvent.SHOW_DEA_BUTTON, isActive);
        Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
      
    }

    /// <summary>
    /// 游戏出牌
    /// </summary>
    /// <param name="dto"></param>
    private void overBro(OverDto dto)
    {
        if(dto.WinUIdList.Contains(Models.GameModel.UserDto.Id)) //胜利
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "");
        else   // 失败
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_EFFECT_AUDIO, "");
        Dispatch(AreaCode.UI, UIEvent.SHOW_OVER_PANEL, dto);
    }

    /// <summary>
    /// 转换出牌
    /// </summary>
    /// <param name="nextUId"></param>
    public void turnBro(int nextUId)
    {
        if (Models.GameModel.UserDto.Id == nextUId)
        {
            //转换出牌
            Dispatch(AreaCode.UI, UIEvent.SHOW_DEA_BUTTON, true);
        }
    }

    /// <summary>
    /// 自己最大不出牌的处理
    /// </summary>
    /// <param name="result"></param>
    public void passResponse(int result)
    {
        bool isActive = false;
        if (result == -1)
        {
            promptMsg.Change("自己最大", Color.red);
            //显示出牌
            isActive = true;
        }
        else if (result==0)
            promptMsg.Change("不出", Color.red);

        Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
        Dispatch(AreaCode.UI, UIEvent.SHOW_DEA_BUTTON, isActive);
    }
}
