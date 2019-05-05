using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISingleManager : SingletonMono<UISingleManager>
{



    #region 加载 UI

    public void GetObjByName(string name, params IMessage[] message)
    {
        UISingleCache.Instance.GetObjByName(name, message);
    }
    public void GetObjByName(string[] names, List<IMessage[]> message)
    {
        for (int i = 0; i < names.Length; i++)
        {
            GetObjByName(names[i], message[i]);
        }
    }
    public void GetObjByName(List<string> names, List<IMessage[]> message)
    {
        for (int i = 0; i < names.Count; i++)
        {
            GetObjByName(names[i], message[i]);
        }
    }

    #endregion



    #region 添加 移除事件的处理

    private Dictionary<int, List<UIABBase>> registeredDic = new Dictionary<int, List<UIABBase>>();

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="mainKey"></param>
    /// <param name="ui"></param>
    public void Add(int[] mainKey, UIABBase ui)
    {
        for (int i = 0; i < mainKey.Length; i++)
        {
            Add(mainKey[i], ui);
        }
    }
    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="mainkey"></param>
    /// <param name="ui"></param>
    public void Add(int mainkey, UIABBase ui)
    {
        if (!registeredDic.ContainsKey(mainkey))
            registeredDic[mainkey] = new List<UIABBase>();
        registeredDic[mainkey].Add(ui);
    }


    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="mainkey"></param>
    /// <param name="ui"></param>
    public void Reomve(int[] mainkey, UIABBase ui)
    {
        for (int i = 0; i < mainkey.Length; i++)
            Reomve(mainkey[i], ui);
    }
    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="mainkey"></param>
    /// <param name="ui"></param>
    public void Reomve(int mainkey, UIABBase ui)
    {
        if (registeredDic.ContainsKey(mainkey))
        {
            if (registeredDic[mainkey].Contains(ui))
            {
                registeredDic[mainkey].Remove(ui);
                if (registeredDic[mainkey].Count == 0)
                    registeredDic.Remove(mainkey);
            }
        }
    }

    #endregion

    #region 触发事件

    public void Execute(int mainkey, object message)
    {
        if (registeredDic.ContainsKey(mainkey))
        {
            List<UIABBase> list = registeredDic[mainkey];
            for (int i = 0; i < list.Count; i++)
                list[i].Execute(mainkey, message);
        }
    }

    #endregion
}
