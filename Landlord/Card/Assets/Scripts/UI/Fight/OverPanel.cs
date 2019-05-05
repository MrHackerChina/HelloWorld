using Protocol.Code;
using Protocol.Constant;
using Protocol.Dto.Fight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverPanel : UIBase {

    private void Awake()
    {
        Bind(UIEvent.SHOW_OVER_PANEL);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SHOW_OVER_PANEL:
                refreshPanel(message as OverDto);
                break;
            default:
                break;
        }
    }

    private Text txtWinIentity;
    private Text txtWinBeen;
    private Button btnBack;

    // Use this for initialization
    void Start () {
        txtWinIentity = TransformHelp.Find<Text>(transform, "txtWinIentity");
        txtWinBeen = TransformHelp.Find<Text>(transform, "txtWinBeen");
        btnBack = TransformHelp.Find<Button>(transform, "btnBack");
        btnBack.onClick.AddListener(backClick);
        gameObject.SetActive(false);

    }

    /// <summary>
    /// 刷新显示
    /// </summary>
    public void refreshPanel(OverDto dto)
    {
        setPanelActive(true);
        txtWinIentity.text = Identity.GetString(dto.WinIdentity);

        if (dto.WinUIdList.Contains(Models.GameModel.UserDto.Id))
        {
            txtWinIentity.text += "胜利";
            txtWinBeen.text = "欢乐豆：+";
        }
        else
        {
            txtWinIentity.text += "失败";
            txtWinBeen.text = "欢乐豆：-";
        }
        txtWinBeen.text += dto.BeenCount;
    }

    private void backClick()
    {
        LoadSceneMsg msg = new LoadSceneMsg(1,
            () =>
            {
               SocketMsg socketMsg = new SocketMsg(OpCode.USER,UserCode.GET_INFO_CREQ,null);
                Dispatch(AreaCode.NET, 0, socketMsg);
            }
            );
        Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, msg);
    }
}
