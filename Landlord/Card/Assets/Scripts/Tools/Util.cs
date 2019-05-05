using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class Util  {
    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string MD5File(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
    public static string MD5Dic(string dics)
    {
        List<string> files = new List<string>();
        List<byte> retVal = new List<byte>();
        RecursiveDir(dics, ref files);
        for (int i = 0; i < files.Count; i++)
        {
            FileStream fs = new FileStream(files[i], FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            retVal.AddRange(md5.ComputeHash(fs));
            fs.Close();
        }
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Count; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string MD5String(string str)
    {
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        string ret = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)), 4, 8);
        return ret.Replace("-", "");
    }

    /// <summary>
    /// 得到Path文件夹下所有文件 并放入allFilePath List中
    /// </summary>
    public static void RecursiveDir(string path, ref List<string> allFilePath, bool isFirstRun = true)
    {
        if (isFirstRun && allFilePath.Count > 0)
        {
            allFilePath.TrimExcess();
            allFilePath.Clear();
        }
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);

        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;

            allFilePath.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            RecursiveDir(dir, ref allFilePath, false);
        }

    }

    /// <summary>
    /// 获取一个路径下的所有文件夹名字
    /// </summary>
    /// <param name="path"></param>
    /// <param name="allFolderPath"></param>
    /// <param name="isClear"></param>
    public static void GetFolder(string path, ref List<string> allFolderPath, bool isClear = true)
    {
        if (isClear && allFolderPath.Count > 0)
        {
            allFolderPath.TrimExcess();
            allFolderPath.Clear();
        }

        string[] dirs = Directory.GetDirectories(path);
        for (int i = 0; i < dirs.Length; i++)
        {
            allFolderPath.Add(dirs[i].Replace('\\', '/'));
        }

        for (int i = 0; i < dirs.Length; i++)
        {
            GetFolder(dirs[i], ref allFolderPath, false);
        }
    }

    /// <summary>
    /// 判断文件是否打开或暂用   true为正在使用  false为没有使用
    /// </summary>
    /// <param name="fileName">文件的路径</param>
    /// <returns></returns>
    public static bool IsFileInUse(string fileName)
    {
        bool isUse = true;
        FileStream fs = null;
        try
        {
            fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            isUse = true;
        }
        catch (System.Exception ex)
        {
        	
        }
        finally
        {
            if (fs != null)
                fs.Close();
        }
        return isUse;
    }



    public static Dictionary<string,string> GetFileExtension(string path , string ext)
    {
        Dictionary<string, string> res = new Dictionary<string, string>();
        string[] files = Directory.GetFiles(path);
        string file = string.Empty;
        for (int i = 0; i < files.Length; i++)
        {
            file = files[i].Trim();
            string e = Path.GetExtension(file);
            if(ext.Equals(e))
            {
                string base64= File.ReadAllText(file);
                string name = Path.GetFileNameWithoutExtension(file);
                //Debug.LogError("!!!!!!!!!!" + name + "@@@@@@@@@@@");
                if (!res.ContainsKey(name))
                {
                    res.Add(name, base64);
                  
                }
                else
                {
                    Debug.LogError("write pic name error ");
                }
            }

        }
        return res;
    }


    public static string GetPathFileName(string path,string name)
    {
        string res = string.Empty;
        List<string> pathName = new List<string>();
        RecursiveDir(path, ref pathName);

        string key = string.Empty;
        for (int i = 0; i < pathName.Count; i++)
        {
            key = Path.GetFileName(pathName[i]);
            if(string.Equals(key,name))
            {
                res = pathName[i].Trim();
                break;
            }
        }

        return res;
    }


    public static string GetDirectoryDir(string path, string name)
    {
        string res = string.Empty;
        string[] dirs = Directory.GetDirectories(path);
        for (int i = 0; i < dirs.Length; i++)
        {
            string[] keys= Directory.GetDirectories(dirs[i]);
            for (int k = 0; k < keys.Length; k++)
            {
                string value = keys[k].Trim();
                if(value.EndsWith(name))
                {
                    res = value;
                    break;
                }
            }
        }
        return res;
    }
    

    /// <summary>
    /// 获取所有字符串中没有重复的字符
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static List<string> GetSameNumber(List<string> args)
    {
        List<string> res = new List<string>();
        string temp = string.Empty;
        for (int i = 0; i < args.Count; i++)
        {
            char[] strCount = args[i].ToCharArray();
            for (int k = 0; k < strCount.Length; k++)
            {
                temp = strCount[k].ToString();
                if(!res.Contains(temp))
                {
                    res.Add(temp);
                }
            }
        }
        return res;
    }


}
