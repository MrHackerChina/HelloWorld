using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  消息处理中心
///  只负责消息的转发
///  
///  UI-> msgCenter -> character
/// </summary>
public class MsgCenter : MonoBase
{
    public static MsgCenter Instance = null;
    private void Awake()
    {
        Instance = this;
        //gameObject.AddComponent<AudioManager>();
        gameObject.AddComponent<UIManager>();
        gameObject.AddComponent<CharacterManager>();
        gameObject.AddComponent<NetManager>();
        gameObject.AddComponent<SceneMgr>();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 发送消息 系统里面所有的发消息 都是通过这个方法来发
    /// 怎么转发 根据不同的模块 来发给不同的模块
    /// 用areaCode来识别不同的模块
    /// 
    /// 
    /// </summary>
    /// <param name="areaCode"> 事件码 用来区分 做什么事情的</param>
    /// <param name="eventCode"> 识别具体做哪些事</param>
    /// <param name="message"> 内容</param>
    public void Dispatch(int areaCode,int eventCode,object message)
    {
        switch (areaCode)
        {
            case AreaCode.AUDIO:
                //AudioManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.CHARACTER:
                CharacterManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.NET:
                NetManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.GAME:
                break;
            case AreaCode.UI:
                UIManager.Instance.Execute(eventCode, message);
                break;
            case AreaCode.SCENE:
                SceneMgr.Instance.Execute(eventCode, message);
                break;
            default:
                break;

        }
    }
    
}
