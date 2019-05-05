using Protocol.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MatchPanel : UIBase
{

    private Button btnMatch;
    private Image imaBg;
    private Text txtDes;
    private Button btnCancel;
    private Button btnEnter;

    private SocketMsg socketMsg;
    private void Awake()
    {
        Bind(UIEvent.SHOW_ENTER_ROOM_BUTTON);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SHOW_ENTER_ROOM_BUTTON:
                btnEnter.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    // Use this for initialization
    void Start()
    {
        btnMatch = TransformHelp.Find<Button>(transform, "btnMatch");
        imaBg = TransformHelp.Find<Image>(transform, "imaBg");
        txtDes = TransformHelp.Find<Text>(transform, "txtDes");
        btnCancel = TransformHelp.Find<Button>(transform, "btnCancel");
        btnEnter = TransformHelp.Find<Button>(transform, "btnEnter");
        btnMatch.onClick.AddListener(matchClick);
        btnCancel.onClick.AddListener(cancelClick);
        btnEnter.onClick.AddListener(enterClick);
        socketMsg = new SocketMsg();

        setobjectActive(false);
        btnEnter.gameObject.SetActive(false);
    }

    private void matchClick()
    {
        socketMsg.Change(OpCode.MATCH, MatchCode.ENTER_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
        setobjectActive(true);
        isMatch = true;
        this.btnMatch.interactable = false;
        //TODO  发送服务器匹配的请求

    }

    private void cancelClick()
    {
        socketMsg.Change(OpCode.MATCH, MatchCode.LEAVE_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);
        setobjectActive(false);
        this.btnMatch.interactable = true;
        //TODO 离开服务器
    }

    private void enterClick()
    {
        Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, new LoadSceneMsg(2, null));
    }

    private void Update()
    {
        if (isMatch)
        {
            timer += Time.deltaTime;
            if (timer >= intervalTime)
            {
                doAnimation();
                timer = 0f;
            }
        }

    }
    public override void OnDestory()
    {
        base.OnDestory();
        btnMatch.onClick.RemoveListener(matchClick);
        btnCancel.onClick.RemoveListener(cancelClick);
        btnEnter.onClick.RemoveListener(enterClick);
    }
    private void setobjectActive(bool active)
    {
        imaBg.gameObject.SetActive(active);
        txtDes.gameObject.SetActive(active);
        btnCancel.gameObject.SetActive(active);
        btnEnter.gameObject.SetActive(active);
    }

    private string defaultText = "正在寻找房间";
    private int dotCount = 0;
    private bool isMatch = false;
    private float intervalTime = 1f;
    private float timer = 0f;
    private void doAnimation()
    {
        txtDes.text = defaultText;
        dotCount++;
        if (dotCount > 5)
            dotCount = 1;
        for (int i = 0; i < dotCount; i++)
        {
            txtDes.text += ".";
        }
    }
}
