using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Dto;
using Protocol.Dto.Fight;

public class StatePanel : UIBase
{

    protected UserDto userDto;
    protected Image imgIdentity;
    protected Text txtReady;
    protected Image imgChat;
    protected Text txtSession;
    // Use this for initialization
    protected virtual void Start()
    {
        imgIdentity = TransformHelp.Find<Image>(transform, "imgIdentity");
        txtReady = TransformHelp.Find<Text>(transform, "txtReady");
        imgChat = TransformHelp.Find<Image>(transform, "imgChat");
        txtSession = TransformHelp.Find<Text>(transform, "txtSession");
        txtReady.gameObject.SetActive(false);
        imgChat.gameObject.SetActive(false);
        setIdentity(1);
    }
    protected virtual void Awake()
    {
        Bind(UIEvent.PLAYER_LEAVE,
            UIEvent.PLAYER_HIDE_SETATE,
            UIEvent.PLAYER_READY,
            UIEvent.PLAYER_ENTER,
            UIEvent.PLAYER_CHAT,
            UIEvent.PLAYER_CHANGE_IDENTITY);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PLAYER_LEAVE:
                if (userDto == null)
                    break;
                int user = (int)message;
                if (userDto.Id == user)
                    setPanelActive(false);
                break;
            case UIEvent.PLAYER_HIDE_SETATE:
                txtReady.gameObject.SetActive(false);
                break;
            case UIEvent.PLAYER_READY:
                if (userDto == null)
                    break;
                int userId = (int)message;
                //如果是自身角色
                if (userDto.Id == userId)
                    readyState();
                //txtReady.gameObject.SetActive(true);
                break;
            case UIEvent.PLAYER_ENTER:
                if (userDto == null)
                    break;
                int id = (int)message;
                if (userDto.Id == id)
                    setPanelActive(true);
                break;
            case UIEvent.PLAYER_CHAT:
                if (userDto == null)
                    break;
                ChatMsg chatMsg = message as ChatMsg;
                if (userDto.Id == chatMsg.UserId)
                    showChat(chatMsg.Text);
                break;
            case UIEvent.PLAYER_CHANGE_IDENTITY:
                if (userDto == null)
                    break;
                if (userDto.Id == (int)message)
                    setIdentity(0);
                break;
            default:
                break;
        }
    }
    protected virtual void readyState()
    {
        txtReady.gameObject.SetActive(true);
    }

    /// <summary>
    /// 设置身份
    /// </summary>
    /// <param name="identity"></param>
    protected void setIdentity(int identity)
    {
        string identityStr = identity == 0 ? "dz" : "nm";
        imgIdentity.sprite = Resources.Load<Sprite>("Identity/" + identityStr);
    }

    protected float showTime = 2f;

    protected float timer = 0f;

    protected bool isShow = false;

    protected void Update()
    {
        if (isShow)
        {
            timer += Time.deltaTime;
            if (timer >= showTime)
            {
                setChatActive(false);
                timer = 0f;
                isShow = false;
            }
        }
    }

    protected void setChatActive(bool active)
    {
        txtReady.gameObject.SetActive(active);
    }

    protected void showChat(string text)
    {
        txtSession.text = text;
        setChatActive(true);
        isShow = true;
    }
}
