using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHelp {


    public static Transform Find(Transform tf, string name)
    {
        Transform child = tf.Find(name);
        if (child != null)
            return child;

        Transform go = null;
        for (int i = 0; i < tf.childCount; i++)
        {
            child = tf.GetChild(i);
            go = Find(child, name);
            if (go != null)
                return go;
        }
        return null;
    }
    public static T Find<T>(Transform tf,string name) where T :Object
    {
        Transform child = tf.Find(name);
        if (child != null)
        {
            return child.GetComponent<T>();
        }

        Transform go = null;
        for (int i = 0; i < tf.childCount; i++)
        {
            child = tf.GetChild(i);
            go = Find(child, name);
            if (go != null)
            {
                return go.GetComponent<T>();
            }
        }
        return null;
    }
	
}
