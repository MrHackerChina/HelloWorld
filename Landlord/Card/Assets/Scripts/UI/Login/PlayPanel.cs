using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : UIBase {


    private Button btnStart;
    private Button btnRegist;

	// Use this for initialization
	void Start () {
        btnStart = TransformHelp.Find<Button>(transform, "btnStart");
        btnRegist = TransformHelp.Find<Button>(transform, "btnRegist");
        btnStart.onClick.AddListener(starClick);
        btnRegist.onClick.AddListener(registClick);
    }

    private void starClick()
    {
        Dispatch(AreaCode.UI, UIEvent.START_PANEL_ACTIVE, true);
    }

    private void registClick()
    {
        Dispatch(AreaCode.UI, UIEvent.REGIST_PANEL_ACTIVE, true);
    }

    public override void OnDestory()
    {
        base.OnDestory();
        btnStart.onClick.RemoveListener(starClick);
        btnRegist.onClick.RemoveListener(registClick);
    }
}
