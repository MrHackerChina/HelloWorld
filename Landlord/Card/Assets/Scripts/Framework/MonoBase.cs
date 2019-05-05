using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBase : MonoBehaviour
{
    public Dictionary<int, List<MonoBase>> dict = new Dictionary<int, List<MonoBase>>();
    public virtual void Execute(int eventCode, object message)
    {
    }
    public void Add(int id, MonoBase manager) {

        if (!dict.ContainsKey(id))
            dict[id] = new List<MonoBase>();
        dict[id].Add(manager);

    }

}
