using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UIRoot : MonoBehaviour
{

  
    private void OnGUI()
    {
        if(GUILayout.Button("",GUILayout.Width(100),GUILayout.Height(20)))
        {
            UISingleManager.Instance.GetObjByName("PlayPanel");
            // UISingleManager.Instance.GetObjByName("RegistPanel");
            //Test();
        }

    }

    void Test()
    {
        Type type = Type.GetType("EventBase");
        ConstructorInfo info = type.GetConstructor(Type.EmptyTypes);
        object objs = info.Invoke(new object[] { });
        print(objs == null);
        MethodInfo met = type.GetMethod("Test");
        met.Invoke(objs, new object[] { "1"});

    }

}
