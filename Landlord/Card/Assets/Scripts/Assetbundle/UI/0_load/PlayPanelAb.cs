using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanelAb : UIABBase
{
    private Button btnStart;
    private Button btnRegist;

    private IMessage message;
    protected override void Awake()
    {
        base.Awake();
        message = new IMessage();
    }

    void Start()
    {
        btnStart = TransformHelp.Find<Button>(transform, "btnStart");
        btnRegist = TransformHelp.Find<Button>(transform, "btnRegist");
        btnStart.onClick.AddListener(btnStartClick);
        btnRegist.onClick.AddListener(btnRegistClick);
    }

    private void btnStartClick()
    {
        message.Change(EventMono.UI, UICode.SHOW_STARTPANEL, true);
        UISingleManager.Instance.GetObjByName("StartPanel", message);
    }

    private void btnRegistClick()
    {
        message.Change(EventMono.UI, UICode.SHOW_REGISPANEL, true);
        UISingleManager.Instance.GetObjByName("RegistPanel", message);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
