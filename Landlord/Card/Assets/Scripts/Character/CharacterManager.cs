using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public static CharacterManager Instance = null;

    private Dictionary<int, List<CharacterBase>> registeredDic = new Dictionary<int, List<CharacterBase>>();

    private void Awake()
    {
        Instance = this;
    }

    public void Add(int[] id, CharacterBase manager)
    {
        for (int i = 0; i < id.Length; i++)
            Add(id[i], manager);
    }

    public void Add(int id, CharacterBase manager)
    {
        if (!registeredDic.ContainsKey(id))
            registeredDic[id] = new List<CharacterBase>();
        registeredDic[id].Add(manager);
    }
    public void Remove(int[] eventCode, CharacterBase manager)
    {
        for (int i = 0; i < eventCode.Length; i++)
            Remove(eventCode[i], manager);
    }

    public void Remove(int id , CharacterBase manager)
    {
        if (registeredDic.ContainsKey(id))
            registeredDic.Remove(id);
    }

    public  void Execute(int eventCode, object message)
    {
        if (!registeredDic.ContainsKey(eventCode))
        {
            Debug.LogWarning("没有注册事件：" + eventCode);
            return;
        }

        List<CharacterBase> list = registeredDic[eventCode];
        for (int i = 0; i < list.Count; i++)
            list[i].Exectue(eventCode, message);

    }
}
