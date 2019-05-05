using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不修改
/// </summary>
public class UIBase : MonoBase {


    private List<int> list = new List<int>();
    /// <summary>
    /// 绑定一个或多个消息
    /// </summary>
    /// <param name="eventCodes"></param>
    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        UIManager.Instance.Add(list.ToArray(), this);
    }
    /// <summary>
    /// 解除绑定的消息
    /// </summary>
    protected void UnBind()
    {
        UIManager.Instance.Remove(list.ToArray(), this);
        list.Clear();
    }

  
    public virtual void OnDestory()
    {
        if (list != null)
            UnBind();
    }
    
    public void Dispatch(int areaCode,int eventCode,object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }

    protected void setPanelActive(bool active)
    {
        gameObject.SetActive(active);
    }
    
}
