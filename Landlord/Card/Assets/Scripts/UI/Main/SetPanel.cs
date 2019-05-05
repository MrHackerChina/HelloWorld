using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetPanel : UIBase {


    private Button btnSet;
    private Image imgBg;
    private Button btnClose;
    private Text txtAudio;
    private Toggle togAudio;
    private Text txtVolum;
    private Slider sldVolume;
    private Button btnQuit;


    // Use this for initialization
    void Start () {
        btnSet = TransformHelp.Find<Button>(transform, "btnSet");
        imgBg = TransformHelp.Find<Image>(transform, "imgBg");
        btnClose = TransformHelp.Find<Button>(transform, "btnClose");
        txtAudio = TransformHelp.Find<Text>(transform, "txtAudio");
        togAudio = TransformHelp.Find<Toggle>(transform, "togAudio");
        txtVolum = TransformHelp.Find<Text>(transform, "txtVolum");
        sldVolume = TransformHelp.Find<Slider>(transform, "sldVolume");
        btnQuit = TransformHelp.Find<Button>(transform, "btnQuit");

        setObjectActive(false);

        btnSet.onClick.AddListener(btnSetClick);
        btnClose.onClick.AddListener(btnCloseClick);
        btnQuit.onClick.AddListener(btnQuitClick);
        togAudio.onValueChanged.AddListener(toAudioChange);
        sldVolume.onValueChanged.AddListener(sldVolumeChange);
    }

    public override void OnDestory()
    {
        base.OnDestory();
        btnSet.onClick.RemoveListener(btnSetClick);
        btnClose.onClick.RemoveListener(btnCloseClick);
        btnQuit.onClick.RemoveListener(btnQuitClick);
        togAudio.onValueChanged.RemoveListener(toAudioChange);
        sldVolume.onValueChanged.RemoveListener(sldVolumeChange);
    }

    void setObjectActive(bool active)
    {
        imgBg.gameObject.SetActive(active);
        btnClose.gameObject.SetActive(active);
        txtAudio.gameObject.SetActive(active);
        togAudio.gameObject.SetActive(active);
        txtVolum.gameObject.SetActive(active);
        sldVolume.gameObject.SetActive(active);
        btnQuit.gameObject.SetActive(active);
    }
    private void btnSetClick()
    {
        setObjectActive(true);
    }

    private void btnCloseClick()
    {
        setObjectActive(false);
    }

    private void btnQuitClick()
    {
        Application.Quit();
    }
    /// <summary>
    /// 音效
    /// </summary>
    /// <param name="result"></param>
    private void toAudioChange(bool value)
    {
        //
        if (value)
            Dispatch(AreaCode.AUDIO, AudioEvent.PLAY_BG_AUDIO, null);
        else
            Dispatch(AreaCode.AUDIO, AudioEvent.STOP_BG_AUDIO, null);
    }
    /// <summary>
    /// 调节声音大小
    /// </summary>
    /// <param name="value"></param>
    private void sldVolumeChange(float value)
    {
        //
        Dispatch(AreaCode.AUDIO, AudioEvent.SET_BG_VOLUM, value);

    }


}
