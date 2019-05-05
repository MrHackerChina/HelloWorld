using System;
using System.Collections.Generic;

public class AudioManager:MonoBase
{
    public const int PLAY_AUDIO = 0;
    public static AudioManager Instance;
    void Awake()
    {
        Instance = this;
    }
}

