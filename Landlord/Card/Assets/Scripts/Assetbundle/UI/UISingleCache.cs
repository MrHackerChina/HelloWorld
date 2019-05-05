using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 加载物体的缓冲
/// </summary>
public class UISingleCache : SingletonMono<UISingleCache>
{

    private Transform uiParent = null;

    private Dictionary<string, GameObject> loadObjDic;
    private void Awake()
    {
        uiParent = GameObject.Find("UIRoot").GetComponent<RectTransform>();
        needObjQueue = new Queue<string>();
        loadObjDic = new Dictionary<string, GameObject>();
        callBackDic = new Dictionary<string, List<IMessage>>();
    }
    private bool isBeginLoad = true;
    private Queue<string> needObjQueue = null;
    private string tempName = string.Empty;

    private Dictionary<string, List<IMessage>> callBackDic;

    public void GetObjByName(string name,params IMessage[] messsage)
    {
        if (messsage!=null)
            addInitBack(name, new List<IMessage>(messsage));
        if (isLoad(name))
        {
            if (messsage != null)
                callInitBack(name);
            return;
        }
        if (isBeginLoad)
        {
            //正在加载
            isBeginLoad = false;
            AssetbundleLoad.Instance.getPrefabByName(name, setParent);
        }
        else
        {
            needObjQueue.Enqueue(name);
        }
    }

    /// <summary>
    /// 设置父物体
    /// </summary>
    /// <param name="obj"></param>
    private void setParent(GameObject obj)
    {
        obj.transform.SetParent(uiParent,false);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.rotation = Quaternion.identity;
        Add(obj);
        callInitBack(obj.name);
        if (needObjQueue.Count > 0)
        {
            tempName = needObjQueue.Dequeue();
            isBeginLoad = true;
            GetObjByName(tempName);
        }
        else
            isBeginLoad = true;
    }

    /// <summary>
    /// 添加加载UI后注册的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="message"></param>
    private void addInitBack(string name,List<IMessage> message)
    {
        if(!callBackDic.ContainsKey(name))
            callBackDic[name] = new List<IMessage>();
        callBackDic[name].AddRange(message);
    }

    /// <summary>
    /// 触发加载UI后注册的事件
    /// </summary>
    /// <param name="name"></param>
    private void callInitBack(string name)
    {
        if (callBackDic.ContainsKey(name))
        {
            List<IMessage> list = callBackDic[name];
            IMessage temp = new IMessage();
            for (int i = 0; i < list.Count; i++)
            {
                temp = list[i];
                EventBase.Instance.Dispatch(temp.mainkey, temp.subkey, temp.message);
            }
            callBackDic.Remove(name);
        }
    }


    /// <summary>
    /// 是否加载过
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private bool isLoad(string name)
    {
        return loadObjDic.ContainsKey(name);
    }

    /// <summary>
    /// 添加物体
    /// </summary>
    /// <param name="go"></param>
    private void Add(GameObject go)
    {
        loadObjDic.Add(go.name, go);
    }

    /// <summary>
    /// 销毁物体
    /// </summary>
    /// <param name="go"></param>
    public void Remove(string name)
    {
        loadObjDic.Remove(name);
        UnLaod(name);
    }

    /// <summary>
    /// 去卸载Ab
    /// </summary>
    /// <param name="name"></param>
    private void UnLaod(string name)
    {
        AssetbundleLoad.Instance.UnLoadAB(name);
    }
}

