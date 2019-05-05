using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour {

    private List<int> list = new List<int>();

    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        CharacterManager.Instance.Add(list.ToArray(), this);
    }
    /// <summary>
    /// 解除绑定的消息
    /// </summary>
    protected void UnBind()
    {
        CharacterManager.Instance.Remove(list.ToArray(), this);
        list.Clear();
    }

    public virtual void Exectue(int eventCode, object messagge) { }

    public void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }
    public virtual void OnDestory()
    {
        if (list != null)
            UnBind();
    }

}
