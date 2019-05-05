using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : UIBase
{
    private Image imaHead;
    private Text txtName;
    private Text txtLv;
    private Slider sldExp;
    private Text txtExp_L;
    private Text txtBeen;

    private void Awake()
    {
        Bind(UIEvent.REFRESH_INFO_PANEL);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REFRESH_INFO_PANEL:
                UserDto user = message as UserDto;
                refreshView(user.Name,user.Lv,user.Exp,user.Been);
                break;
            default:
                break;
        }
    }
    // Use this for initialization
    void Start()
    {
        imaHead = TransformHelp.Find<Image>(transform, "imaHead");
        txtName = TransformHelp.Find<Text>(transform, "txtName");
        txtLv = TransformHelp.Find<Text>(transform, "txtLv");
        sldExp = TransformHelp.Find<Slider>(transform, "sldExp");
        txtExp_L = TransformHelp.Find<Text>(transform, "txtExp_L");
        txtBeen = TransformHelp.Find<Text>(transform, "txtBeen");
    }

    /// <summary>
    /// 刷新视图
    /// </summary>
    private void refreshView(string name,int lv,int exp,int been)
    {
        txtName.text = name;
        txtLv.text = "Lv." + lv;
        txtExp_L.text = exp + "/" + lv * 100;
        sldExp.value = (float)exp / (lv * 100);
        txtBeen.text = "×" + been;
    }
}

