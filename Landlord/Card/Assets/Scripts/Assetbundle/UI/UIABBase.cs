using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIABBase : MonoBehaviour {

    protected virtual void Awake()
    {
    }
    protected virtual void OnDestroy()
    {
        UISingleCache.Instance.Remove(this.gameObject.name);
        UnBind();
    }
    private List<int> list = new List<int>();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="keys"></param>
    protected void Bind(params int[] keys)
    {
        list.AddRange(keys);
        UISingleManager.Instance.Add(keys, this);
    }

    private void UnBind()
    {
        UISingleManager.Instance.Reomve(list.ToArray(), this);
        list.Clear();
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="mainkey"></param>
    /// <param name="subkey"></param>
    /// <param name="message"></param>
    public  virtual void  Execute(int subkey, object message)
    {

    }
    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="mainkey"></param>
    /// <param name="subkey"></param>
    /// <param name="message"></param>
    protected void Dispatch(int mainkey,int subkey,object message)
    {
        EventBase.Instance.Dispatch(mainkey, subkey, message);
    }


    public void SetPanelActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
