using Protocol;
using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase {

    private Button butReigist;
    private Button butClose;
    private InputField inputAccount;
    private InputField inputPassword;
    private InputField inputRepeat;
    private PromptMsg promptMsg;
    private SocketMsg socketMsg;
    private void Awake()
    {
        Bind(UIEvent.REGIST_PANEL_ACTIVE);
    }
    public override void Execute(int eventCode, object message)
    {
       switch(eventCode)
        {
            case UIEvent.REGIST_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }


    void Start()
    {
        butReigist = TransformHelp.Find<Button>(transform, "butReigist");
        butClose = TransformHelp.Find<Button>(transform, "butClose");
        inputAccount = TransformHelp.Find<InputField>(transform, "inputAccount");
        inputPassword = TransformHelp.Find<InputField>(transform, "inputPassword");
        inputRepeat = TransformHelp.Find<InputField>(transform, "inputRepeat");
        butReigist.onClick.AddListener(reigistClick);
        butClose.onClick.AddListener(closeClick);
        promptMsg = new PromptMsg();
        socketMsg = new SocketMsg();
        setPanelActive(false);
    }
    private AccountDto dto = new AccountDto();
    //private SocketMsg msg = new SocketMsg();

    private void reigistClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Change("注册用户名不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text) ||
            inputPassword.text.Length < 4 || inputPassword.text.Length > 16)
        {
            promptMsg.Change("注册的密码不合法", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputRepeat.text) || inputPassword.text != inputRepeat.text)
        {
            promptMsg.Change("2次输入的密码不正确", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        //if (string.IsNullOrEmpty(inputAccount.text) || string.IsNullOrEmpty(inputPassword.text) ||
        //    inputPassword.text.Length < 4 || inputPassword.text.Length > 16 || 
        //    string.IsNullOrEmpty(inputRepeat.text)|| inputPassword.text != inputRepeat.text)
        //    return;

        //登陆
        AccountDto dto = new AccountDto(inputAccount.text, inputPassword.text);
        socketMsg.Change(OpCode.ACCOUNT, AccountCode.REGIST_CREQ, dto);

        //dto.Account = inputAccount.text;
        //dto.Password = inputPassword.text;
        //msg.OpCode = OpCode.ACCOUNT;
        //msg.SubCode = AccountCode.REGIST_CREQ;
        //msg.Value = dto;


        Dispatch(AreaCode.NET, 0, socketMsg);
    }
    private void closeClick()
    {
        setPanelActive(false);
    }
    public override void OnDestory()
    {
        base.OnDestory();
        butReigist.onClick.RemoveListener(reigistClick);
        butClose.onClick.RemoveListener(closeClick);
    }
}
