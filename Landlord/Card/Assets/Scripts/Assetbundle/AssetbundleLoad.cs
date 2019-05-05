using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AssetbundleLoad : SingletonMono<AssetbundleLoad>
{


    private Dictionary<string, string> gameConfig = new Dictionary<string, string>();

    private string parent = "/Prefabe/";
    private string manifest = "/Prefabe/Prefabe";
    private AssetBundleManifest abManiFest;

    /// <summary>
    /// 加载过依赖的集合   名称  --  次数
    /// </summary>
    private Dictionary<string, uint> dependenABDic = new Dictionary<string,uint>();
    /// <summary>
    /// 加载过model ab的集合   名称  -  ab
    /// </summary>
    private Dictionary<string, AssetBundle> abDependenDic = new Dictionary<string, AssetBundle>();

    public void Test()
    {
        getPrefabByName("RegistPanel");
    }


    public void getPrefabByName(string name, Action<GameObject> back = null)
    {
        if (!gameConfig.ContainsKey(name))
            initConfig();
        if (!gameConfig.ContainsKey(name))
            Debug.Log("打包错误");
        // string file = Application.streamingAssetsPath + parent + gameConfig[name];

        StartCoroutine(loadPrefabByName(name,back));
    }

    private IEnumerator loadPrefabByName(string modelName,Action<GameObject> back=null)
    {
        string name = gameConfig[modelName];
        if (abManiFest == null)
            yield return StartCoroutine(loadGameManifest());
        string[] dependen = abManiFest.GetAllDependencies(name);
        //Debug.LogError(dependen.Length);
        WWW www = null;
        string dep = string.Empty;
        for (int i = 0; i < dependen.Length; i++)
        {
            dep = dependen[i];
            if (isEverLoad(dep))
            {
                addDepABDict(dep);
                continue;
            }
            www = new WWW("file://" + Application.streamingAssetsPath + parent + dep);
            yield return www;
            if(!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("加载错误"+www.error);
                yield break;
            }
            addDepABDict(dep, www.assetBundle);
            www.Dispose();

        }
        www = new WWW("file://" + Application.streamingAssetsPath + parent + name);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("加载错误");
            yield break;
        }
        AssetBundleRequest abs = www.assetBundle.LoadAllAssetsAsync();
        yield return abs;

        //记录
        addDepABDict(modelName, www.assetBundle);

        var model = abs.asset;
        GameObject go = Instantiate(model) as GameObject;
        go.name = modelName;
        if (back != null)
            back(go);
        www.Dispose();

    }

    /// <summary>
    /// 是否加载过
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool isEverLoad(string name)
    {
        return dependenABDic.ContainsKey(name);
    }

    /// <summary>
    /// 记录 AB 可以卸载
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ab"></param>
    void addDepABDict(string name, AssetBundle ab = null)
    {
        if (!dependenABDic.ContainsKey(name))
            dependenABDic[name] = 0;
        dependenABDic[name]++;
        if (ab == null)
            return;
        if (abDependenDic.ContainsKey(name))
            abDependenDic.Add(name, ab);
        else
            abDependenDic[name] = ab;

    }

    /// <summary>
    /// 初始化对应打包的文件路径
    /// </summary>
    private void initConfig()
    {
        List<string> paths = new List<string>();
        Util.RecursiveDir(Application.streamingAssetsPath, ref paths);
        string model = string.Empty;
        for (int i = 0; i < paths.Count; i++)
        {
            if (Path.GetFileName(paths[i]) == "modeconfig.txt")
                model = paths[i];
        }
        if (string.IsNullOrEmpty(model))
        {
            Debug.Log("model 错误");
            return;
        }
        string line = string.Empty;
        string mainKey = string.Empty;
        StringReader sr = new StringReader(File.ReadAllText(model));
        while ((line = sr.ReadLine()) != null)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            if (line.EndsWith("}"))
            {
                continue;
            }
            if (line.EndsWith("{"))
            {
                mainKey = line.Substring(0, line.Length - 1);
                gameConfig[mainKey] = "";
            }
            else
            {
                gameConfig[mainKey] = line;
            }
        }
    }


    /// <summary>
    /// 加载所有文件的mainfest
    /// </summary>
    /// <returns></returns>
    private IEnumerator loadGameManifest()
    {
        string asset = Application.streamingAssetsPath + manifest;
        WWW www = new WWW("file://" + asset);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError("加载错误 manifest");
            yield break;
        }
        AssetBundle ab = www.assetBundle;
        abManiFest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        ab.Unload(false);
    }


    /// <summary>
    /// 卸载AB
    /// </summary>
    /// <param name="name"></param>
    public void UnLoadAB(string name)
    {
        if(abDependenDic.ContainsKey(name))
        {
            abDependenDic[name].Unload(true);
            abDependenDic.Remove(name);
        }

        string[] dep= abManiFest.GetAllDependencies(name);
        string temp = string.Empty;
        for (int i = 0; i < dep.Length; i++)
        {
            temp = dep[i];
            if(dependenABDic.ContainsKey(temp))
            {
                dependenABDic[temp]--;
                if(dependenABDic[temp]==0)
                {

                }
            }
        }
      
    }
}
