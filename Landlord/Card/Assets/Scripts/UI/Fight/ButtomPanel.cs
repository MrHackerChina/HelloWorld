using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtomPanel : UIBase
{

    private void Awake()
    {
        Bind(UIEvent.CHANGE_MUTIPLE);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CHANGE_MUTIPLE:
                changeMutiple((int)message);
                break;
            default:
                break;
        }
    }
    private Text txtBeen;
    private Text txtMutiple;
    private Image imgChoose;
    private Button btnChat;
    private Button[] btns;
    private SocketMsg soketMsg;
    // Use this for initialization
    void Start()
    {
        soketMsg = new SocketMsg();
        txtBeen = TransformHelp.Find<Text>(transform, "txtBeen");
        txtMutiple = TransformHelp.Find<Text>(transform, "txtMutiple");
        btnChat = TransformHelp.Find<Button>(transform, "btnChat");
        btns = new Button[7];
        imgChoose = TransformHelp.Find<Image>(transform, "imgChoose");
        for (int i = 0; i < 7; i++)
        {
            btns[i] = imgChoose.transform.GetChild(i).GetComponent<Button>();
            int index = i + 1;
            btns[i].onClick.AddListener(() => { chatClick(index); });
        }
        btnChat.onClick.AddListener(setChooseActive);
        imgChoose.gameObject.SetActive(false);
        refreshPanel(Models.GameModel.UserDto.Been);
    }

    public override void OnDestory()
    {
        base.OnDestory();
        btnChat.onClick.RemoveListener(setChooseActive);
        for (int i = 0; i < 7; i++)
        {
            btns[i].onClick.RemoveAllListeners();
        }
    }
    /// <summary>
    /// 刷线自身的面板
    /// </summary>
    private void refreshPanel(int beenCount)
    {
        this.txtBeen.text = "×" + beenCount;
    }
    private void changeMutiple(int muti)
    {
        this.txtMutiple.text = "×" + muti;
    }
    private void setChooseActive()
    {
        bool active = imgChoose.gameObject.activeInHierarchy;
        imgChoose.gameObject.SetActive(!active);
    }

    private void chatClick(int chatType)
    {
        soketMsg.Change(OpCode.CHAT, ChatCode.DEFAULT, chatType);
        Dispatch(AreaCode.NET, 0, soketMsg);
    }
}
