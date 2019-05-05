using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMono<UIManager>
{

    //public static UIManager Instance = null;



    //private void Awake()
    //{
    //    Instance = this;
    //}

    public void Execute(int eventCode, object message)
    {
        if (registeredDic.ContainsKey(eventCode))
        {
            foreach (var item in registeredDic[eventCode])
            {
                if (item != null)
                    item.Execute(eventCode, message);
            }
        }
        //registeredDic[eventCode].Execute(eventCode,message);
    }

    private Dictionary<int, List<UIBase>> registeredDic = new Dictionary<int, List<UIBase>>();
    public void Add(int[] v, UIBase uIBase)
    {
        for (int i = 0; i < v.Length; i++)
        {
            // registeredDic.Add(v[i], uIBase);
            Add(v[i], uIBase);
        }
    }

    public void Add(int v, UIBase uiBase)
    {
        List<UIBase> list = null;
        if (!registeredDic.ContainsKey(v))
        {
            list = new List<UIBase>();
            list.Add(uiBase);
            registeredDic.Add(v, list);
            return;
        }
        list = registeredDic[v];
        list.Add(uiBase);
    }
    public void Remove(int[] v, UIBase uIBase)
    {
        for (int i = 0; i < v.Length; i++)
        {
            if (registeredDic.ContainsKey(v[i]))
                registeredDic.Remove(v[i]);
        }
    }
}
