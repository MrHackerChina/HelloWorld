using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{

    static T _instance;
    static string name = "SingletonMono";
    static GameObject single = null;
    /// <summary>
    /// 线程锁
    /// </summary>
    static object locker = new object();

    public static T Instance
    {
        get
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    if (single == null)
                    {
                        single = GameObject.Find(name);
                        if (single == null)
                            single = new GameObject("SingletonMono");
                        DontDestroyOnLoad(single);
                    }
                    _instance = single.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

}

