using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// 编码的工具类 粘包和粘包
/// </summary>
public static class EncodeTool
{
    #region  粘包和拆包的问题
    /// <summary>
    /// 构造消息体
    /// </summary>
    /// <returns></returns>
    public static byte[] EncodePacket(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(data.Length);
                bw.Write(data);
                byte[] byteArray = new byte[(int)ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, byteArray, 0, (int)ms.Length);
                return byteArray;
            }
        }

    }

    /// <summary>
    /// 解析数据
    /// </summary>
    /// <returns></returns>
    public static byte[] DecodePacket(ref List<byte> dataCache)
    {
        if (dataCache.Count < 4)
            return null;
        //throw new Exception("数据缓存长度不足4");
        using (MemoryStream ms = new MemoryStream(dataCache.ToArray()))
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                int length = br.ReadInt32();
                int dataRemainLength = (int)(ms.Length - ms.Position);
                if (length > dataRemainLength)
                    return null;
                //throw new Exception("数据长度不够包头约定的长度 不能构成一个完整的消息");
                byte[] data = br.ReadBytes(length);
                //更新一下数据缓存
                dataCache.Clear();
                dataCache.AddRange(br.ReadBytes(dataRemainLength));
                return data;
            }
        }
    }
    #endregion


    #region  构造发送的SocketMsg类

    /// <summary>
    /// 把socketmsg类转换成字节数组 发送出去
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] EncodeMeg(SocketMsg msg)
    {
        MemoryStream ms = new MemoryStream();
        BinaryWriter bw = new BinaryWriter(ms);
        bw.Write(msg.OpCode);
        bw.Write(msg.SubCode);
        //如果不为null  才需要把object 转换成字节数据
        if (msg.Value != null)
        {
            byte[] values = EncodeObj(msg.Value);
            bw.Write(values);
        }
        byte[] data = new byte[ms.Length];
        Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);
        bw.Close();
        ms.Close();
        return data;
    }

    /// <summary>
    /// 将收到的字节数组转换成SocketMsg
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static SocketMsg DecodeMsg(byte[] data)
    {
        MemoryStream ms = new MemoryStream(data);
        BinaryReader br = new BinaryReader(ms);
        SocketMsg msg = new SocketMsg();
        msg.OpCode = br.ReadInt32();
        msg.SubCode = br.ReadInt32();
        if (ms.Length > ms.Position)
        {
            byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));
            object value = DecodeObj(valueBytes);
            msg.Value = value;
        }
        br.Close();
        ms.Close();
        return msg;
    }

    #endregion


    #region  把object类转换成byte[]
    /// <summary>
    /// 序列化对象
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] EncodeObj(object value)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, value);
            byte[] byt = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, byt, 0, (int)ms.Length);
            return byt;
        }
    }

    /// <summary>
    /// 反序列对象
    /// </summary>
    /// <param name="valueBytes"></param>
    /// <returns></returns>
    public static object DecodeObj(byte[] valueBytes)
    {
        using (MemoryStream ms = new MemoryStream(valueBytes))
        {
            BinaryFormatter bf = new BinaryFormatter();
            object value = bf.Deserialize(ms);
            return value;
        }
    }
    #endregion
}

