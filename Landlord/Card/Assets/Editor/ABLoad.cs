using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class ABLoad 
{
    [MenuItem("AssetBundle/load")]
    static void StreamAssetsLoad()
    {
        //获取路径下的所有文件
        string path = Application.dataPath + "/Prefabe";
        List<string> filePaths = new List<string>();
        getAllPath(path,".prefab" ,ref filePaths);
        getABDependencies(filePaths);
    }

    static void getAllPath(string path, string ext,ref List<string> filePaths)
    {
        string[] files = Directory.GetFiles(path);
        string temp = string.Empty;
        for (int i = 0; i < files.Length; i++)
        {
            temp = files[i];
            if (Path.GetExtension(temp) == ext)
            {
                temp = temp.Replace(Application.dataPath+"/", "");
                temp = temp.Replace("\\", "/");
                filePaths.Add(temp);
            }
        }
        //获取文件夹
        string[] dirs = Directory.GetDirectories(path);
        for (int i = 0; i < dirs.Length; i++)
        {
            getAllPath(dirs[i], ext, ref filePaths);
        }
    }


    static void getABDependencies(List<string> files)
    {
        string temp = string.Empty;
        for (int i = 0; i < files.Count; i++)
        {
            temp = "Assets/" + files[i];
            string[] deps = AssetDatabase.GetDependencies(temp);
            for (int k = 0; k < deps.Length; k++)
            {
                Debug.Log(deps[k]);
            }
        }
      
    }
}
