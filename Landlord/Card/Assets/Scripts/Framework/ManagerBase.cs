using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase : MonoBase{

   
    public override void Execute(int eventCode, object message) {
        if(!dict.ContainsKey(eventCode))
        {
            Debug.LogWarning("没有注册事件：" + eventCode);
            return;
        }

        List<MonoBase> list = dict[eventCode];
        for (int i = 0; i < list.Count; i++)
            list[i].Execute(eventCode, message);

    }

   
}
