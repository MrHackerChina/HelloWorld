using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanelAb : UIABBase {

    protected override void Awake()
    {
        base.Awake();
        Bind(UICode.SHOW_STARTPANEL);
    }
    public override void Execute(int subkey, object message)
    {
        switch (subkey)
        {
            case UICode.SHOW_STARTPANEL:
                SetPanelActive((bool)message);
                break;
            default:
                break;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }


    private Button butClose;
    private void Start()
    {
        butClose = TransformHelp.Find<Button>(transform, "butClose");


        butClose.onClick.AddListener(() =>
        { SetPanelActive(false); });
    }


}
