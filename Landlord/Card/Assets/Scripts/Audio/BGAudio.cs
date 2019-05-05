using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAudio : AudioBase {


    private void Awake()
    {
        Bind(AudioEvent.PLAY_BG_AUDIO,
            AudioEvent.SET_BG_VOLUM,
            AudioEvent.STOP_BG_AUDIO);
    }
    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.PLAY_BG_AUDIO:
                playAudio();
                break;
            case AudioEvent.SET_BG_VOLUM:
                setAudio((int)message);
                break;
            case AudioEvent.STOP_BG_AUDIO:
                stopAudio();
                break;
            default:
                break;
        }
    }
    private AudioSource audioSource;

    void Start () {
        audioSource = GetComponent<AudioSource>();

    }
	
	private void playAudio()
    {
        audioSource.Play();
    }

    private void setAudio(int value)
    {
        audioSource.volume = value;
    }

    private void stopAudio()
    {
        audioSource.Stop();
    }
}
