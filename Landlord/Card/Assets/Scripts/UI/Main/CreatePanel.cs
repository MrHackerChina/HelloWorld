using Protocol.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel : UIBase {

    private InputField inputName;
    private Button btnCreate;
    private PromptMsg promptMsg;
    private void Awake()
    {
        Bind(UIEvent.CREATE_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CREATE_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }



    // Use this for initialization
    void Start () {
        inputName = TransformHelp.Find<InputField>(transform, "inputName");
        btnCreate = TransformHelp.Find<Button>(transform, "btnCreate");
        promptMsg = new PromptMsg();
        btnCreate.onClick.AddListener(btnCreateClick);
    }

    public override void OnDestory()
    {
        base.OnDestory();
        btnCreate.onClick.RemoveListener(btnCreateClick);

    }
    private void btnCreateClick()
    {
        if(string.IsNullOrEmpty(inputName.text))
        {
            promptMsg.Change("输入错误", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        SocketMsg msg = new SocketMsg(OpCode.USER, UserCode.CREATE_CREQ, inputName.text);
        Dispatch(AreaCode.NET, 0, msg);
       
    }
    
}
