using Protocol.Code;
using Protocol.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStatePanel : StatePanel
{

    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SHOW_CRAB_BUTTON,
            UIEvent.SHOW_DEA_BUTTON,
            UIEvent.SET_MY_PLAYER_DARA,
            UIEvent.PLAY_HIDE_READ_BUTTON);
    }

    public override void Execute(int eventCode, object message)
    {
        base.Execute(eventCode, message);
        switch (eventCode)
        {
            case UIEvent.SET_MY_PLAYER_DARA:
                this.userDto = message as UserDto;
                break;
            case UIEvent.SHOW_CRAB_BUTTON:
                bool active = (bool)message;
                btnGrab.gameObject.SetActive(active);
                btnNGrab.gameObject.SetActive(active);
                break;
            case UIEvent.SHOW_DEA_BUTTON:
                bool activ = (bool)message;
                //btnDeal.gameObject.SetActive(activ);
                //btnNDeal.gameObject.SetActive(activ);
                stateDeatBtn(activ);
                break;
            case UIEvent.PLAY_HIDE_READ_BUTTON:
                btnReady.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    private Button btnDeal;
    private Button btnNDeal;
    private Button btnGrab;
    private Button btnNGrab;
    private Button btnReady;

    private SocketMsg socktMsg;
    protected override void Start()
    {
        base.Start();
        btnDeal = TransformHelp.Find<Button>(transform, "btnDeal");
        btnNDeal = TransformHelp.Find<Button>(transform, "btnNDeal");
        btnGrab = TransformHelp.Find<Button>(transform, "btnGrab");
        btnNGrab = TransformHelp.Find<Button>(transform, "btnNGrab");
        btnReady = TransformHelp.Find<Button>(transform, "btnReady");

        btnDeal.onClick.AddListener(dealClick);
        btnNDeal.onClick.AddListener(nDealClick);
        btnGrab.onClick.AddListener(
            () =>
            {
                grabClick(true);
            }
            );
        btnNGrab.onClick.AddListener(
             () =>
             {
                 grabClick(false);
             }
            );
        btnReady.onClick.AddListener(readyClick);

        socktMsg = new SocketMsg();
        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
        //btnDeal.gameObject.SetActive(false);
        //btnNDeal.gameObject.SetActive(false);
        stateDeatBtn(false);
        //自身角色
        UserDto myUserDto = Models.GameModel.MatchRoomDto.UIdUserDict[Models.GameModel.UserDto.Id];
        //发送
        this.userDto = myUserDto;
    }

    /// <summary>
    /// 准备
    /// </summary>
    protected override void readyState()
    {
        base.readyState();
        btnReady.gameObject.SetActive(false);
    }


    #region  点击事件
    /// <summary>
    /// 出牌
    /// </summary>
    private void dealClick()
    {
        Dispatch(AreaCode.CHARACTER, CharacterEvent.DEAL_CARD, null);
        //要在回调中隐藏
        //stateDeatBtn(false);
    }

    /// <summary>
    /// 不出牌
    /// </summary>
    private void nDealClick()
    {
        socktMsg.Change(OpCode.FIGHT, FightCode.PASS_CREQ, null);
        Dispatch(AreaCode.NET, 0, socktMsg);
        //stateDeatBtn(false);
    }

    /// <summary>
    /// 是否抢地主
    /// </summary>
    /// <param name="v"></param>
    private void grabClick(bool result)
    {
        socktMsg.Change(OpCode.FIGHT, FightCode.GRAB_LANDLORD_CREQ, result);
        Dispatch(AreaCode.NET, 0, socktMsg);
        btnGrab.gameObject.SetActive(false);
        btnNGrab.gameObject.SetActive(false);
    }
    /// <summary>
    /// 准备
    /// </summary>
    private void readyClick()
    {
        socktMsg.Change(OpCode.MATCH, MatchCode.READY_CREQ, null);
        Dispatch(AreaCode.NET, 0, socktMsg);
    }
    #endregion

    private void stateDeatBtn(bool active)
    {
        btnDeal.gameObject.SetActive(active);
        btnNDeal.gameObject.SetActive(active);
    }


    public override void OnDestory()
    {
        base.OnDestory();
        btnDeal.onClick.RemoveAllListeners();
        btnNDeal.onClick.RemoveAllListeners();
        btnGrab.onClick.RemoveAllListeners();
        btnNGrab.onClick.RemoveAllListeners();
        btnReady.onClick.RemoveAllListeners();
    }
}
