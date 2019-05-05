using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Dto;
using Protocol.Code;
using Protocol;

public class StartPanel : UIBase
{

    private Button butLogin;
    private Button butClose;
    private InputField inputAccount;
    private InputField inputPassword;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;

    private void Awake()
    {
        Bind(UIEvent.START_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch(eventCode)
        {
            case UIEvent.START_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        butLogin = TransformHelp.Find<Button>(transform, "butLogin");
        butClose = TransformHelp.Find<Button>(transform, "butClose");
        inputAccount = TransformHelp.Find<InputField>(transform, "inputAccount");
        inputPassword = TransformHelp.Find<InputField>(transform, "inputPassword");
        butLogin.onClick.AddListener(loginClick);
        butClose.onClick.AddListener(closeClick);
        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
        setPanelActive(false);
    }

    private void loginClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Change("用户名不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if(string.IsNullOrEmpty(inputPassword.text))
        {
            promptMsg.Change("登录的密码不能为空", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (inputPassword.text.Length < 4 || inputPassword.text.Length > 16)
        {
            promptMsg.Change("登录的密码不合法 应为4-16位", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        //登陆 需要和服务器交互
        // 

        AccountDto dto = new AccountDto(inputAccount.text, inputPassword.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.LOGIN, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);

    }
    private void closeClick()
    {
       setPanelActive(false);
    }
    public override void OnDestory()
    {
        base.OnDestory();
        butLogin.onClick.RemoveListener(loginClick);
        butClose.onClick.RemoveListener(closeClick);
    }
}
