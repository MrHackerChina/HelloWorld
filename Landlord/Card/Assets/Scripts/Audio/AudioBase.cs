using System;
using System.Collections.Generic;

public class AudioBase:MonoBase
{
    List<int> list = new List<int>();
    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        //AudioManager.Instance.Add(list, this);
    }
    protected void UnBind()
    {
        //AudioManager.Instance.Remove(list.ToArray(), this);
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
}

