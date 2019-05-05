using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnGUI()
    {
        if(GUILayout.Button("",GUILayout.Width(100),GUILayout.Height(200)))
        {
            AssetbundleLoad.Instance.Test();
        }
    }
}
