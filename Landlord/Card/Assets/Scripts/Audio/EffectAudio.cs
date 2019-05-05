using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectAudio:AudioBase
{
    private void Awake()
    {
        Bind(AudioEvent.PLAY_EFFECT_AUDIO);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.PLAY_EFFECT_AUDIO:
                PlayEffectAudio(message.ToString());
                break;
            default:
                break;
        }
    }

    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayEffectAudio(string assetName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sound/" + assetName);
        audioSource.clip = clip;
        audioSource.Play();
    }

}
