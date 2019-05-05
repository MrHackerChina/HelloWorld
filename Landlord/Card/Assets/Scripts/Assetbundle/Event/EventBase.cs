using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EventBase : SingletonMono<EventBase>
{
    public void Dispatch(int mainkey, int subkey, object message)
    {
        //Type type = Type.GetType("UISingleManager");
        //ConstructorInfo info = type.GetConstructor(Type.EmptyTypes);
        //object objs = info.Invoke(new object[] { });
        //print(objs == null);
        //MethodInfo met = type.GetMethod("Execute");
        //met.Invoke(objs, new object[] { subkey, message });
        UISingleManager.Instance.Execute(subkey, message);
    }

    public void Test(string mono)
    {
        print(1111111111111);
        print("@@@" + mono);
    }

}