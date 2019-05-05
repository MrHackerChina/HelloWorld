using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class PackageSingleGame : MonoBehaviour {

    static string gameFile = "/Prefabe/";


    static string packageManifestFileName= "checklist.txt";

    static BuildTarget builTarget = BuildTarget.StandaloneWindows64;


    [MenuItem("PackageGame/packageGame")]
    static void PackageGame()
    {
        string path = Application.dataPath + gameFile;
        if (!Directory.Exists(path))
        {
            Debug.LogError("本地没有游戏文件");
            return;
        }
        string[] files = Directory.GetDirectories(path);
        print(files.Length);
        Dictionary<string, uint> assetRef = new Dictionary<string, uint>();
        Dictionary<string, List<string>> assDepenDetail = new Dictionary<string, List<string>>();
        List<AssetBundleBuild> mult = new List<AssetBundleBuild>();
        string modelPerfab = null;
        string[] depens = null;
        string depenAssets = string.Empty;
        //
        List<string> textPath = new List<string>();

        #region  打包
        for (int i = 0; i < files.Length; i++)
        {
            List<string> asset = new List<string>();
            Util.RecursiveDir(files[i], ref asset);
            for (int k = 0; k < asset.Count; k++)
            {
                modelPerfab = asset[k].Replace("\\", "/");
                string ext = Path.GetExtension(modelPerfab);
                if (ext.Equals(".txt"))
                {
                    string str = modelPerfab.Replace(Application.dataPath, "");
                    Debug.LogError(str + "!!!!!!!!!!!!");
                    if (!textPath.Contains(str))
                    {
                        textPath.Add(str);
                    }
                    continue;
                }
                //if (ext.Equals(".mp4"))
                //{
                //    string str = modelPerfab.Replace(Application.dataPath, "");
                //    Debug.LogError(str);
                //    if (!textPath.Contains(str))
                //    {
                //        textPath.Add(str);
                //    }
                //    continue;
                //}
                modelPerfab = "Assets" + modelPerfab.Replace(Application.dataPath, "");
                depens = AssetDatabase.GetDependencies(modelPerfab);
                for (int j = 0; j < depens.Length; j++)
                {
                    depenAssets = depens[j];
                    if (!assetRef.ContainsKey(depenAssets))
                    {
                        assetRef.Add(depenAssets, 0);
                    }
                    else
                    {
                        assetRef[depenAssets]++;
                    }
                    if (!assDepenDetail.ContainsKey(depenAssets))
                    {
                        assDepenDetail[depenAssets] = new List<string>();
                    }
                    assDepenDetail[depenAssets].Add(modelPerfab);

                }
            }
        }
        var enu = assDepenDetail.GetEnumerator();
        while (enu.MoveNext())
        {
            if(enu.Current.Key.EndsWith(".dll")) continue;
            if (enu.Current.Value.Count > 1 && !enu.Current.Key.EndsWith(".cs"))
            {
                Debug.Log(enu.Current.Key + "---" + enu.Current.Value.Count);
                AssetBundleBuild build = new AssetBundleBuild();
                //复制依赖
                build.assetNames = new string[] { enu.Current.Key };
                //返回不具有扩展名的指定路径字符串的文件名
                build.assetBundleName = Path.GetFileNameWithoutExtension(enu.Current.Key);
                mult.Add(build);
            }
        }


        for (int j = 0; j < files.Length; j++)
        {
            files[j] = files[j].Replace("\\", "/");
            print(files[j]);
            string[] str = files[j].Split('/');
            string dir = str[str.Length - 1];

            List<string> allModels = new List<string>();
            Util.RecursiveDir(files[j], ref allModels);
            for (int k = 0; k < allModels.Count; k++)
            {
                modelPerfab = allModels[k].Replace("\\", "/");
                string ext = Path.GetExtension(modelPerfab);
                if (ext.Equals(".txt"))
                {
                    string strPath = modelPerfab.Replace(Application.dataPath, "");
                    Debug.LogError(strPath + "!!!!!!!!!!!!!!!");
                    if (!textPath.Contains(strPath))
                    {
                        textPath.Add(strPath);
                    }
                    continue;
                }
                modelPerfab = "Assets" + modelPerfab.Replace(Application.dataPath, "");
                var name = Path.GetFileNameWithoutExtension(modelPerfab);
                name = dir + "/" + name;
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = name;
                print(name);
                build.assetNames = new string[] { modelPerfab };
                // abs[i] = build;
                mult.Add(build);
                Debug.Log(allModels[k]);
            }
        }
        /* string dllFile = Application.dataPath + "/Plugins" + gameFile + "GameByte.bytes";
          string gameDll = dllFile.Replace("\\", "/");
          gameDll= "Assets" + dllFile.Replace(Application.dataPath, "");
          if (!File.Exists(dllFile))
          {
              Debug.LogError("没有游戏的Dll");
              return;
          }
          string dllName = Path.GetFileNameWithoutExtension(dllFile);
          AssetBundleBuild dllBuild = new AssetBundleBuild();
          dllBuild.assetBundleName = dllName;
          dllBuild.assetNames = new string[] { gameDll };
          Debug.LogError(dllName);
          mult.Add(dllBuild);*/
        string targetPath = Application.streamingAssetsPath + gameFile;

        if (Directory.Exists(targetPath))
        {
            Directory.Delete(targetPath, true);
        }
        Directory.CreateDirectory(targetPath);
        BuildPipeline.BuildAssetBundles(targetPath, mult.ToArray(), BuildAssetBundleOptions.None, builTarget);
        AssetDatabase.Refresh();
        #endregion

        #region  保存文本

        Debug.LogError(textPath.Count + "}}}}}}}}}}}}}}}");
        if (textPath.Count > 0)
        {
            for (int i = 0; i < textPath.Count; i++)
            {
                string textDir = Path.GetDirectoryName(Application.streamingAssetsPath + textPath[i].ToLower());
                print(textDir);
                print(textPath[i]);
                string textCopy = Application.dataPath + textPath[i];
                string textFileTarget = Application.streamingAssetsPath + textPath[i].ToLower();
                if (!Directory.Exists(textDir))
                {
                    Directory.CreateDirectory(textDir);
                }
                if (File.Exists(textFileTarget))
                {
                    File.Delete(textFileTarget);
                }
                FileInfo info = new FileInfo(textCopy);
                if (info.Exists)
                {
                    info.CopyTo(textFileTarget, true);
                }
            }

        }

        AssetDatabase.Refresh();
        #endregion
    }

    [MenuItem("PackageGame/CreateGamePath")]
    static void CreateGamePath()
    {
        string path = Application.dataPath + gameFile;
       
        // string modelconfig = Application.streamingAssetsPath + "/ModelAssets/Two";
        if (!Directory.Exists(path))
        {
            Debug.LogError("本地没有游戏文件");
            return;
        }
        //string[] games = Directory.GetDirectories(targetPath);
        string abName = string.Empty;
        string abPath = string.Empty;
        string targetPath = Application.streamingAssetsPath + gameFile;
        string[] files = Directory.GetDirectories(path);
        StreamWriter sw = new StreamWriter(targetPath + "/modeconfig.txt");
        for (int i = 0; i < files.Length; i++)
        {
            List<string> modelPath = new List<string>();
            Util.RecursiveDir(files[i], ref modelPath);
            files[i] = files[i].Replace("\\", "/");
            string[] str = files[i].Replace(path,"").Split('/');
            string dir = str[0];
            for (int k = 0; k < modelPath.Count; k++)
            {
                //Debug.LogError(files[i]);
                abName = Path.GetFileNameWithoutExtension(modelPath[k]);
                Debug.LogError(abName);
                sw.WriteLine(abName + "{");
                abPath = modelPath[k].Replace(files[i].Replace("\\", "/") + "/", "");
                abPath = abPath.Replace(Path.GetExtension(abPath), "");
                Debug.LogError(dir + "/" + abName);
                //if (abName.Equals("textvalue"))
                //{
                //    sw.WriteLine("\t" + (dir + "/" + abName));
                //}
                //else {
                  sw.WriteLine("\t" + (dir + "/" + abName).ToLower());
               // }
                sw.WriteLine("}");
                if (files[i].Contains("Model_Effect"))
                {
                    continue;
                }
                string effect = path + "Model_Effect/" + abName + "_effect/";
                if (Directory.Exists(effect))
                {
                    List<string> effectModel = new List<string>();
                    Util.RecursiveDir(effect, ref effectModel);
                    if(effectModel.Count>0)
                    {
                        sw.WriteLine(abName + "_effect" + "{");
                        string dep = string.Empty;
                        for (int j = 0; j < effectModel.Count; j++)
                        {
                            dep = effectModel[j].Replace(effect, "").Trim();
                            dep = dep.Replace(Path.GetExtension(dep), "").Trim();
                            sw.WriteLine("\t" + dep);
                        }
                        sw.WriteLine("}");
                    }

                }

            }
        }
        string videoPath = targetPath+"/video";
        if(Directory.Exists(videoPath))
        {
            List<string> videoPaths = new List<string>();
            string key = string.Empty;
            string value = string.Empty;
            Util.RecursiveDir(videoPath, ref videoPaths);
            for (int i = 0; i < videoPaths.Count; i++)
            {
                string ext= Path.GetExtension(videoPaths[i]);
                if (!string.Equals(ext,".mp4"))
                {
                    continue;
                }
                videoPaths[i] = videoPaths[i].Replace("\\", "/");
                key = Path.GetFileNameWithoutExtension(videoPaths[i]);
                sw.WriteLine(key + "{");
                value = videoPaths[i].Replace(targetPath + "/", "");
                sw.WriteLine("\t" + value.ToLower());
                sw.WriteLine("}");
            }

        }
        string make = targetPath + "/make";
        if (Directory.Exists(make))
        {
            List<string> videoPaths = new List<string>();
            string key = string.Empty;
            string value = string.Empty;
            Util.RecursiveDir(make, ref videoPaths);
            for (int i = 0; i < videoPaths.Count; i++)
            {
                string ext = Path.GetExtension(videoPaths[i]);
                if (string.Equals(ext, ".meta"))
                {
                    continue;
                }
                videoPaths[i] = videoPaths[i].Replace("\\", "/");
                key = Path.GetFileNameWithoutExtension(videoPaths[i]);
                sw.WriteLine(key + "{");
                value = videoPaths[i].Replace(targetPath + "/", "");
                sw.WriteLine("\t" + value.ToLower());
                sw.WriteLine("}");
            }
        }
        sw.Close();
        sw.Dispose();
        AssetDatabase.Refresh();
        GeneratorChecklist(targetPath);
    }

    static void GeneratorChecklist(string path)
    {
        string newFilePath = path + packageManifestFileName;
        if (File.Exists(newFilePath)) File.Delete(newFilePath);

        List<string> files = new List<string>();
        Util.RecursiveDir(path, ref files);

        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            if (file.EndsWith(".meta") || file.Contains(".DS_Store") || file.EndsWith(".manifest")) continue;
            //Debug.LogError("错误的打包" + files[i]);
            string md5 = Util.MD5File(file);
            string value = file.Replace(path, string.Empty);
            sw.WriteLine(value + "|" + md5);
            Debug.LogError(value + "|" + md5);
        }
        sw.Close(); fs.Close();
        AssetDatabase.Refresh();
    }



    [MenuItem("PackageGame/TestMD5")]
    static void TestMD5()
    {
        string path = "E:/发布文件/factory/0.0.8/screen";
        List<string> refFiles = new List<string>();
        Util.RecursiveDir(path, ref refFiles);
        string newFilePath = path + "/model";
        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < refFiles.Count; i++)
        {
            string file = refFiles[i];
            if (file.EndsWith(".meta") || file.Contains(".DS_Store") || file.EndsWith(".manifest")) continue;
            //Debug.LogError("错误的打包" + files[i]);
            string md5 = Util.MD5File(file);
            string value = file.Replace(path, string.Empty);
            sw.WriteLine(value + "|" + md5);
            Debug.LogError(value + "|" + md5);
        }
        sw.Close(); fs.Close();
        AssetDatabase.Refresh();
    }

    [MenuItem("PackageGame/NoPerfab")]
    static void LoadNoPerfab()
    {
        var selection = Selection.activeObject;
        if (selection == null) return;
        var path = AssetDatabase.GetAssetPath(selection);
        List<AssetBundleBuild> mult = new List<AssetBundleBuild>();
        List<string> files = new List<string>();
        Util.RecursiveDir(path, ref files);
        string name = string.Empty;
        string model = string.Empty;
        for (int i = 0; i < files.Count; i++)
        {
            model = files[i].Replace(Application.dataPath, "");
            name = Path.GetFileNameWithoutExtension(model);
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = name;
            print(name);
            build.assetNames = new string[] { model };
            mult.Add(build);
        }
        print(mult.Count);
        string targetPath = Application.streamingAssetsPath + "/noperfab/";

        if (Directory.Exists(targetPath))
        {
            Directory.Delete(targetPath, true);
        }
        Directory.CreateDirectory(targetPath);
        BuildPipeline.BuildAssetBundles(targetPath, mult.ToArray(), BuildAssetBundleOptions.None, builTarget);
        AssetDatabase.Refresh();
    }
}