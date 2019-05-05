using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : ManagerBase
{
    public static SceneMgr Instance = null;

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        Add(SceneEvent.LOAD_SCENE, this);
    }


    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.LOAD_SCENE:
                LoadSceneMsg msg = message as LoadSceneMsg;
                loadScene(msg);
                break;
            default:
                break;
        }
    }

    private Action OnSceneLoaded = null;

    private void loadScene(LoadSceneMsg msg)
    {
        if (msg.SceneBuildIndex != -1)
            SceneManager.LoadScene(msg.SceneBuildIndex);
        if (msg.SceneBuildName != null)
            SceneManager.LoadScene(msg.SceneBuildName);
        if (msg.OnSceneLoaded != null)
            OnSceneLoaded = msg.OnSceneLoaded;
    }
    /// <summary>
    /// 加载完成的时候调用
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (OnSceneLoaded != null)
        {
            OnSceneLoaded();
            OnSceneLoaded = null;
        }
            
    }

}
